 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


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
    [SerializeField] float groundRadius;
    [SerializeField] Vector3 groundPosition;
    public float groundDistance;
    [SerializeField] LayerMask groundMask;
    [SerializeField] Collider[] checkGround;
    bool groundIsChecked;

    [Header("Move")]

    public float beginSpeed;
    public  float speed;
    float smoothPlayerMove;
    public float smoothSpeedPlayerMove;
    Vector3 positionToMove;
    public float beginAcceleration;

    [Header("PickObjects")]
    bool isPickable = false;
    public bool isPicked = false;
    public GameObject hangingObjectPosition;
    public GameObject otherGameObject;
    public GameObject gameObjectPicked;
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
    [SerializeField] float forceShakeAttack;
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

    public bool isCrouch;

    [SerializeField] bool canSneak;
    [SerializeField] bool canPickUp;
    [SerializeField] bool canAttack;
    [SerializeField] bool canRoll;
    [SerializeField] public bool canMove = true;


    [SerializeField] GameObject lineRenderer;
    ThrowPrediction tp;
    [SerializeField]  Collider[] attackCollider;
    bool attackDuration;

    public bool isControlled;

    bool isPicking;

    [SerializeField] AnimationCurve throwCurve;
    CameraController camController;

    [Header("Audio")]

    [SerializeField] AudioSource[] audioSources;

    [SerializeField] AudioManager am;

    [SerializeField] public Material crystalMat;

    [SerializeField] Renderer crystal;
    bool aimStopMove;
    float rangeFeedbackLerp;
    float rangeFeedTimer = 1.5f;

    public bool inMMRange;
    bool inMMRangeOnce;
    bool onceRangeFeedback;
    Vector4 initialColorMat;
    CapsuleCollider capsuleC;
    public bool canChangeColor = true;
    public bool isControlChangeColor;
    bool onceColorControl;
    [SerializeField] Color controlColor;
    Color currentColor;
    [SerializeField] float controlColorIntensity;
    private void Awake()
    {
        if (gameObject.tag != "Player")
        {
           
            crystalMat = new Material(crystalMat);
            crystal.material = crystalMat;

           

        }

    }

    private void Start()
    {
        //tp = lineRenderer.GetComponent<ThrowPrediction>();
        //aS = GetComponent<AudioSource>();
        initialColorMat = crystalMat.GetVector("_EmissionColor");
        beginSpeed = speed;
        if (gameObject.tag != "Player")
        beginAcceleration = GetComponent<PositionEnemies>().transformPosition.GetComponent<NavMeshAgent>().acceleration;
        anim = GetComponent<Animator>();
        mindPower = GetComponent<MindPower>();
        otherGameObject = null;
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
        camController = cam.GetComponent<CameraController>();
        capsuleC = GetComponent<CapsuleCollider>();
    }

    public void Update()
    {
        
        RangeMMFeedback();
        OnFire();
        isGrounded();
        Attack();
        if (isControlled == true && groundIsChecked == true)
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

        if (Input.GetButton("Fire2") || Input.GetAxis("Fire2") > 0)
        {
            if (MindPower.isMindManipulated == false)
            {
                canMove = false;
            }

        }

        else
        {
            canMove = true;
        }

        if (isPicked == true)
        {
            gameObjectPicked = otherGameObject;
        }

        else gameObjectPicked = null;

        currentColor = crystalMat.color;
    }

    private void FixedUpdate()
    {
        if (isControlled == true)
        {
            Movements();
            
        }

    }

    private void OnDisable()
    {
        crystalMat.SetVector("_EmissionColor", Color.white );
    }

    void RangeMMFeedback()
    {
        
       if (canChangeColor == true && isControlChangeColor == false)
        {
            if (inMMRange == true)
            {
                if (inMMRangeOnce == false)
                {
                    rangeFeedbackLerp = 0;
                    inMMRangeOnce = true;
                    onceColorControl = false;
                }

                crystalMat.SetVector("_EmissionColor", Vector4.Lerp(currentColor, Color.white * 2.25f, rangeFeedbackLerp));
                rangeFeedbackLerp += rangeFeedTimer * Time.deltaTime;

            }

            else
            {
                if (inMMRangeOnce == true)
                {
                    rangeFeedbackLerp = 0;
                    inMMRangeOnce = false;
                    onceColorControl = false;
                }
                crystalMat.SetVector("_EmissionColor", Vector4.Lerp(currentColor * 2.25f, initialColorMat, rangeFeedbackLerp));
                rangeFeedbackLerp += rangeFeedTimer * Time.deltaTime;

            }
        }

      else if (canChangeColor == true && isControlChangeColor == true)
        {
            if (onceColorControl == false)
            {
                rangeFeedbackLerp = 0;
                onceColorControl = true;
            }
            controlColorIntensity = (-0.0875f * ( Vector3.Distance(transform.position, player.transform.position)) + 4.5f);
            //controlColor = controlColor * controlColorIntensity;
            //crystalMat.SetVector("_EmissionColor", Vector4.Lerp(currentColor * 2.25f, controlColor * controlColorIntensity, rangeFeedbackLerp));
            crystalMat.SetVector("_EmissionColor",  controlColor * controlColorIntensity);
            rangeFeedbackLerp += rangeFeedTimer * Time.deltaTime;

        }



        //else if (inMMRange == false)
        //{

        //    crystalMat.SetVector("_EmissionColor", Vector4.Lerp(Color.white, Color.white / 5, rangeFeedbackLerp));
        //    rangeFeedbackLerp += rangeFeedTimer * Time.deltaTime;
        //}
    }

    void StepSoundEvent()
    {
        int randomSound = Random.Range(0, am.steps.Length);
        audioSources[0].PlayOneShot(am.steps[randomSound], .05f);
    }

    void Rotation()
    {
        if (canMove == true)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                if (Input.GetAxis("Fire2") == 0 || MindPower.isMindManipulated == true)
                {

                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationCam.transform.rotation, smoothRotationPlayer);
                    //smoothRotationPlayer += speedRotationPlayer * Time.deltaTime;
                    isAimingRotating = false;


                }
                
            }

            else if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
            {
                //smoothRotationPlayer = 0;
            }

        }


    }



    void Movements()
    {
        if ( canMove == true)
        {
            if (Input.GetAxis("Fire2") == 0 && Input.GetButton("Fire2") == false || MindPower.isMindManipulated == true)
            {
                if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                {
                    if (DetectCollisions() == true && speed != 0)
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

            else if (Input.GetAxis("Fire2") > 0 || Input.GetButton("Fire2") == true  )
            {
                if (MindPower.isMindManipulated == false)
                {

                    anim.SetBool("Run", false);
                    transform.rotation = Quaternion.Euler(new Vector3(0, camZoom.transform.rotation.eulerAngles.y, 0));
                    positionToMove = transform.position;
                }

            }

            
        }

        
            
                
    }
    
   
    void PickUpEvent()
    {
        otherGameObject.GetComponent<ObjectSound>().enabled = true;
        isAxisF1InUse = true;
        otherGameObject.transform.position = hangingObjectPosition.transform.position;
        otherGameObject.GetComponent<Rigidbody>().isKinematic = true;
        otherGameObject.GetComponent<MeshCollider>().isTrigger = true;
        if (otherGameObject.gameObject.tag != "Wood") otherGameObject.GetComponent<Rigidbody>().detectCollisions = false;
        //otherGameObject.GetComponent<Rigidbody>().detectCollisions = false;
        isPickable = false;
        otherGameObject.transform.parent = hangingObjectPosition.transform;
        isPicking = false;


    }

    private void PickUp()
    {
       
            
            if (isPicked == false && canPickUp == true)
                
            {
                if (Input.GetKey(KeyCode.E) || Input.GetButton("Y") )
                {
                isPicking = true;
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
     
        
        //this.transform.GetComponent<CharacterController>().speed *= 3;

        if (otherGameObject.GetComponent<Goo>() != null)
        {
            otherGameObject.GetComponent<Goo>().isGooAble = true;
            
        }
        otherGameObject.transform.parent = null;
        otherGameObject.GetComponent<Rigidbody>().isKinematic = false;
        otherGameObject.GetComponent<MeshCollider>().isTrigger = false;
        //if (otherGameObject.gameObject.tag != "Wood")
            

        otherGameObject.GetComponent<Rigidbody>().AddForce((transform.forward * throwStrengthX) + (transform.up * throwStrengthY));
        timerThrowOffset = Time.time + timerThrow;
        isThrowing = true;
        StartCoroutine(ThrowDelay());


    }

    IEnumerator ThrowDelay()
    {
        yield return new WaitForSeconds(.25f);
        otherGameObject.GetComponent<Rigidbody>().detectCollisions = true;
        yield return new WaitForSeconds(.5f);
        isPicked = false;
        yield return null;
    }
    private void Throw()
    {
        if (cam.transform.eulerAngles.x > 0 && cam.transform.eulerAngles.x < 40)
        {
            throwStrengthY = (-8.75f * cam.transform.eulerAngles.x) + 350;
        }

        if (isPicked == true)
        {

            if (Input.GetKeyUp(KeyCode.E) || Input.GetButtonUp("Y") /*Input.GetAxisRaw("Fire1") == 0*/)
            {

                if (isAxisF1InUse == true)
                {
                   
                        //isPicked = false;
                        isAxisF1InUse = false;
                        anim.SetTrigger("Throw");
                    

                }


                //cam.GetComponent<CameraController>().CameraDeZoomFocus();
            }


            
        }

    }


    //private void Throw()
    //{


    //    if (isPicked == true)
    //    {

    //        if (Input.GetKeyUp(KeyCode.E) || throwStrengthX >= 500 || Input.GetButtonUp("Y") /*Input.GetAxisRaw("Fire1") == 0*/)
    //        {

    //            if (isAxisF1InUse == true)
    //            {
    //                if (throwStrengthX >= 50)
    //                {
    //                    isAxisF1InUse = false;
    //                    anim.SetTrigger("Throw");


    //                }

    //                else if (throwStrengthX < 50)
    //                {
    //                    isPicked = false;
    //                    isAxisF1InUse = false;
    //                    ThrowEvent();
    //                }

    //            }


    //            //cam.GetComponent<CameraController>().CameraDeZoomFocus();
    //        }



    //        else if (Input.GetKey(KeyCode.E) || Input.GetButton("Y") /*Input.GetAxisRaw("Fire1") == 1*/ )

    //        {
    //            //tp.velocity = throwStrengthX / 40;
    //            throwStrengthX += throwStrengh + Time.deltaTime;
    //            throwStrengthY += throwHigh + Time.deltaTime;


    //        }

    //        if (Input.GetButtonDown("Fire1"))
    //        {
    //            if (isAxisF1InUse == false)
    //            {

    //                isAxisF1InUse = true;
    //            }

    //        }
    //    }

    //}

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
    void AttackAnimStop()
    {
        anim.SetBool("Attack", false);

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
                anim.SetBool("Attack",true);

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
                    StartCoroutine(camController.CameraShakeTiming(forceShakeAttack, .25f, .15f));
                    if (hitCollider.GetComponent<PositionEnemies>() != null && hitCollider.GetComponent<PositionEnemies>().transformPosition.GetComponent<FieldOfView>().target == null)
                    {
                        hitCollider.GetComponent<PositionEnemies>().transformPosition.GetComponent<FieldOfView>().target = gameObject;
                        hitCollider.GetComponent<PositionEnemies>().transformPosition.GetComponent<FieldOfView>().visible = true;
                    }
                    
                }

            }

        }
        
        
    }

    private void OnDrawGizmos()
    {
        if (attackDuration == true)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(attackPosition.transform.position, attackRadius);
           
        }

       // Gizmos.DrawSphere(transform.position + groundPosition, groundRadius);
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

    void isGrounded() // Une méthode renvoyant un booléan.
    {

        checkGround = Physics.OverlapSphere(transform.position +  groundPosition, groundRadius, groundMask);
        if (checkGround.Length != 0)

        {
            groundIsChecked = true;
            
        }


        else
        {
            
            anim.Rebind();
            groundIsChecked = false;
        }

        


    }


    IEnumerator Dodge()
    {
        if (Input.GetButtonDown("Jump") && canRoll == true)
        {
            
                rb.velocity += targetRotationCam.transform.forward * 100 * dodgePower * Time.deltaTime;
                anim.SetTrigger("Roll");
                yield return new WaitForSeconds(dodgeTimer);
                rb.velocity = new Vector3(0, 0, 0);
                StopFire();
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
