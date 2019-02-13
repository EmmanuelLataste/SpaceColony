 using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterController : Flammable {
    [Header("Rotation")]
    float horizontal;
    float vertical;
    public float rotationAiming;
    public float speedRotationPlayer;
    public float smoothRotationPlayer;
    public GameObject targetRotationCam;
    bool isAimingRotating = false;

    [Header("Jump")]
    [ SerializeField]
    float jump;
    public float groundDistance;

    [Header("Move")]

    public float beginSpeed;
    public  float speed;
    float smoothPlayerMove;
    public float smoothSpeedPlayerMove;
    Vector3 positionToMove;

    [Header("PickObjects")]
    bool isPickable = false;
    public bool isPicked = false;
    public GameObject hangingObjectPosition;
    public GameObject otherGameObject;
    bool isHolding = false;

    [Header("ThrowObjects")]
    public float throwStrengthX;
    public float throwStrengh;
    public float throwStrengthY;
    public float throwHigh;

    [Header("Animations")]
    public static Animator anim;
    AnimatorStateInfo animStateInfoCrouch;
    int crouchStateHash = Animator.StringToHash("Crouch Layer.Crouch");
    
    [Header("Cameras")]
    public GameObject cam;
    public GameObject camZoom;
    public Camera mainCam;

    [Header("Attack")]
    [SerializeField] GameObject attackPosition;
    [SerializeField] float attackRadius;
    [SerializeField] float attackDamage;
    [SerializeField] float attackOffset;
    float attackTimer;

    Rigidbody rb;
    GameObject player;
    MindPower mindPower;

    public bool isAlive = true;
    bool isAxisF1InUse;

    public float dodgeTimer;
    public float dodgePower;

    float timerThrow = .2f;
    float timerThrowOffset;
    bool isThrowing;

    public LayerMask maskDetection;
    public float lengthDetection;
    RaycastHit hitDetection;
    public float speedPush;
    [SerializeField] float health;

    bool isCrouch;

    [SerializeField] bool canSneak;
    [SerializeField] bool canPickUp;
    [SerializeField] bool canAttack;
    [SerializeField] GameObject lineRenderer;
    ThrowPrediction tp;
    [SerializeField]  Collider[] attackCollider;
    bool attackDuration;

    public bool isControlled;

    private void Start()
    {
        //tp = lineRenderer.GetComponent<ThrowPrediction>();
        beginSpeed = speed;
        anim = GetComponent<Animator>();
        mindPower = GetComponent<MindPower>();
        otherGameObject = null;
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
    }

     public void Update()
    {
        Attack();
        if (isControlled == true)
        {
            horizontal = Input.GetAxis("Horizontal"); // On stocke les valeurs du joystick gauche dans deux variables ( valeurs entre -1 et 1)
            vertical = Input.GetAxis("Vertical");
            Sneak();
            Rotation();
            Death();
            
            StartCoroutine(Dodge());

            if (Time.time >= timerThrowOffset && isThrowing == true)
            {
                otherGameObject.GetComponent<Rigidbody>().detectCollisions = true;
                isThrowing = false;
            }

            if (isPickable == true && isPicked == false)
                PickUp();
            if (isPicked == true && isPickable == false)
                Throw();

            Debug.DrawRay(transform.position + transform.up, transform.forward, Color.yellow);


            anim.SetFloat("Direction", vertical);
            anim = GetComponent<Animator>();
        }
       

        
    }

    private void FixedUpdate()
    {
        if (isControlled == true)
        {
            Movements();
            //StartCoroutine(Dodge());
            Jump();

        }

    }


    void Rotation()
    {

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            if (Input.GetAxis("Fire2") == 0 || MindPower.isMindManipulated == true)
            {
                
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationCam.transform.rotation, smoothRotationPlayer );
                //smoothRotationPlayer += speedRotationPlayer * Time.deltaTime;
                isAimingRotating = false;


            }

            
            //else if (Input.GetAxis("Fire2") > 0 && player.GetComponent<MindPower>().isMindManipulated == false)
            //{
              
            //     if (Input.GetAxis("Horizontal") > 0)
            //     {
                     
            //           transform.Rotate(Vector3.up * rotationAiming * Time.deltaTime, Space.Self);
                     
            //     }

            //     else if (Input.GetAxis("Horizontal") < 0)
            //     {
                    
            //            transform.Rotate(-Vector3.up * rotationAiming * Time.deltaTime, Space.Self);
                    
            //     }

            //     if (Input.GetAxis("Vertical2") > 0)
            //    {
            //        transform.Rotate(Vector3.up * rotationAiming * Time.deltaTime, Space.Self);
            //    }

            //} ( MANETTE CONTROLLER )

        }

        else if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            //smoothRotationPlayer = 0;
        }

       
    }

    void Movements()
    {

        if (Input.GetAxis("Fire2") == 0 && Input.GetButton("Fire2") == false || MindPower.isMindManipulated == true)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                    if (DetectCollisions() == true && speed != 0 )
                {
                    anim.SetBool("Run", true);
                    positionToMove = transform.position + transform.forward * speed * Time.deltaTime;

                    rb.MovePosition(Vector3.Lerp(transform.position, positionToMove, smoothPlayerMove));
                    smoothPlayerMove += smoothSpeedPlayerMove * Time.deltaTime;


                }
                    else if (DetectCollisions() == false)
                {
                    anim.SetBool("Run", false);
                }
                  
            }

            else
            {
               
                    anim.SetBool("Run", false);
                    smoothPlayerMove = 0;
                
            }
        }

        else if (Input.GetAxis("Fire2") > 0 || Input.GetButton("Fire2") == true)
        {
            if (MindPower.isMindManipulated == false)
            {
               
                anim.SetBool("Run", false);
                transform.rotation =Quaternion.Euler(new Vector3( 0, camZoom.transform.rotation.eulerAngles.y, 0));
                positionToMove = transform.position;
            }
           
        }
            
                
    }
    
    void Jump()
    {
        if (isGrounded() == true) // Si la méthode en dessous est vrai, donc si le rayon touche le sol
        {
            if (Input.GetButtonDown("Jump")) // Si on appuit sur Jump
            {
                
                GetComponent<Rigidbody>().AddForce(new Vector3(0, jump * 1000, 0));
                //GetComponent<Rigidbody>().AddForce(new Vector3(horizontal, jump, vertical), ForceMode.Impulse);
                // Alors on ajout une force sur Y pour sauter.
            }
           
        }

    }
    void PickUpEvent()
    {
        isAxisF1InUse = true;
        otherGameObject.transform.position = hangingObjectPosition.transform.position;
        otherGameObject.GetComponent<Rigidbody>().isKinematic = true;
        otherGameObject.GetComponent<MeshCollider>().isTrigger = true;
        if (otherGameObject.gameObject.tag != "Wood") otherGameObject.GetComponent<Rigidbody>().detectCollisions = false;
        //otherGameObject.GetComponent<Rigidbody>().detectCollisions = false;
        isPickable = false;
        otherGameObject.transform.parent = hangingObjectPosition.transform;


    }

    private void PickUp()
    {
       
            
            if (isPicked == false && canPickUp == true)
                
            {
                if (Input.GetKey(KeyCode.E) || Input.GetButton("Y") )
                {
               
                isPicked = true;
                anim.SetTrigger("PickUp");

                    
                 }

                else if (Input.GetKeyUp(KeyCode.E) && Input.GetButtonUp("Y"))
                {
                isAxisF1InUse = false;
                }
                 
        }
          
    }

    void ThrowEvent()

    {
        
        isPicked = false;
        //this.transform.GetComponent<CharacterController>().speed *= 3;
        
        if (otherGameObject.GetComponent<Goo>() != null)
        {
            otherGameObject.GetComponent<Goo>().isGooAble = true;
            
        }
        otherGameObject.transform.parent = null;
        otherGameObject.GetComponent<Rigidbody>().isKinematic = false;
        otherGameObject.GetComponent<MeshCollider>().isTrigger = false;
        if (otherGameObject.gameObject.tag != "Wood") otherGameObject.GetComponent<Rigidbody>().detectCollisions = true;

        otherGameObject.GetComponent<Rigidbody>().AddForce((transform.forward * throwStrengthX) + (transform.up * throwStrengthY));
        timerThrowOffset = Time.time + timerThrow;
        isThrowing = true;
        throwStrengthX = 0;
        throwStrengthY = 0;


    }

    private void Throw()
    {


        if (isPicked == true)
        {

            if (Input.GetKeyUp(KeyCode.E) || throwStrengthX >= 500 || Input.GetButtonUp("Y") /*Input.GetAxisRaw("Fire1") == 0*/)
            {
                if (isAxisF1InUse == true)
                {
                    if (throwStrengthX >= 50)
                    {
                        isAxisF1InUse = false;
                        anim.SetTrigger("Throw");


                    }

                    else if (throwStrengthX < 50)
                    {
                        isPicked = false;
                        isAxisF1InUse = false;
                        ThrowEvent();
                    }
                    
                }


                //cam.GetComponent<CameraController>().CameraDeZoomFocus();
            }



            else if (Input.GetKey(KeyCode.E) || Input.GetButton("Y") /*Input.GetAxisRaw("Fire1") == 1*/ )

            {
                //tp.velocity = throwStrengthX / 40;
                throwStrengthX += throwStrengh + Time.deltaTime;
                throwStrengthY += throwHigh + Time.deltaTime;
                

            }

            if (Input.GetButtonDown("Fire1"))
            {
                if (isAxisF1InUse == false)
                {

                    isAxisF1InUse = true;
                }

            }
        }

    }

    
    private void Sneak()
    {
        if (canSneak == true)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetButtonDown("B") )
            {
              
                    if (isCrouch == false)
                    {
                        anim.SetBool("Crouch", true);
                        speed /= 2.5f;
                        isCrouch = true;
                    }

                    else
                    {
                        anim.SetBool("Crouch", false);
                        speed = beginSpeed;
                        isCrouch = false;
                    }

               
               
            }
        }
    
    }
    void AttackEventStop()
    {
        attackDuration = false;
      
    }

    void AttackEvent()
    {
        attackDuration = true;
        
    }

    
    private void Attack()
    {
        if (Time.time > attackTimer) canAttack = true;

        if (canAttack == true)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetButtonDown("X"))
            {
                attackTimer = Time.time + attackOffset;
                canAttack = false;
                anim.SetTrigger("Attack");

            }
        }

        if (attackDuration == true)
        {

            attackCollider = Physics.OverlapSphere(attackPosition.transform.position, attackRadius);
            foreach (Collider hitCollider in attackCollider)
            {
                if (hitCollider.gameObject.layer == LayerMask.NameToLayer("Entity") && hitCollider.gameObject != this.gameObject && hitCollider is CapsuleCollider)
                {
                    hitCollider.GetComponent<Life>().Damages(attackDamage);

                }

            }

        }
        
        
    }

    private void OnDrawGizmos()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetButtonDown("X"))
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(attackPosition.transform.position, attackRadius);
           
        }
    }

    
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pickable") && isPicked == false)
        {
            
            otherGameObject = other.gameObject;
            isPickable = true;

        }

    }

    public void OnTriggerExit(Collider other)
    {
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Pickable"))
        {
            isPickable = false;

        }
    }

    public bool DetectCollisions()
    {

        if (Physics.Raycast(transform.position + transform.up , transform.forward,out hitDetection,lengthDetection, maskDetection)) 
        {
            return false;
            
        }
        return true;
    }

    bool isGrounded() // Une méthode renvoyant un booléan.
    {

        RaycastHit hit;
        Debug.DrawRay(transform.position, -transform.up * groundDistance, Color.red); // Permet de voir le ray dans la scène lorsque c'est lancé.
        if (Physics.Raycast(transform.position, -transform.up, out hit, groundDistance))
        // Si un rayon de 2f partant la position du player, allant vers le sol( groundDistance) touche un objet ayant le calque " ground "...
        {
            return true; //Alors on renvoit Vrai
        }
        return false;

    }


    IEnumerator Dodge()
    {
        if (Input.GetButtonDown("Jump"))
        {
            
                rb.velocity += targetRotationCam.transform.forward * 100 * dodgePower * Time.deltaTime;
                yield return new WaitForSeconds(dodgeTimer);
                rb.velocity = new Vector3(0, 0, 0);
                if (GetComponent<Ignitable>() == true)
                {
                    Destroy(GetComponent<Ignitable>().particleFires);
                    Destroy(GetComponent<Ignitable>());
                }
            
        }
        yield return null;
    }

    public void Health(float damage)
    {
        health -= damage;
    }

    void Death()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    
}
