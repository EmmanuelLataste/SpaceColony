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
    public float speed;
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

    private void Start()
    {
        beginSpeed = speed;
        anim = GetComponent<Animator>();
        mindPower = GetComponent<MindPower>();
        otherGameObject = null;
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
    }

     public void Update()
    {
        
        animStateInfoCrouch = anim.GetCurrentAnimatorStateInfo(1);
        horizontal = Input.GetAxis("Horizontal"); // On stocke les valeurs du joystick gauche dans deux variables ( valeurs entre -1 et 1)
        vertical = Input.GetAxis("Vertical");

        Rotation();

        if (Time.time >= timerThrowOffset && isThrowing == true)
        {
            otherGameObject.GetComponent<Rigidbody>().detectCollisions = true;
            isThrowing = false;
        }

        if (isPickable == true && isPicked == false)
            StartCoroutine(PickUp());
        if (isPicked == true && isPickable == false)
            Throw();

        Debug.DrawRay(transform.position + transform.up, transform.forward , Color.yellow);
        

        anim.SetFloat("Direction", vertical);
        anim = GetComponent<Animator>();

        
    }

    private void FixedUpdate()
    {
        Movements();
        //StartCoroutine(Dodge());
        Jump();
    
    }


    void Rotation()
    {

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            if (Input.GetAxis("Fire2") == 0 || MindPower.isMindManipulated == true)
            {
                
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationCam.transform.rotation, smoothRotationPlayer);
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
                    if (DetectCollisions() == true)
                {
                    anim.SetBool("Run", true);
                    positionToMove = transform.position + transform.forward * speed * Time.deltaTime;
                    GetComponent<Rigidbody>().MovePosition(Vector3.Lerp(transform.position, positionToMove, smoothPlayerMove));
                    smoothPlayerMove += smoothSpeedPlayerMove * Time.deltaTime;


                }

                    else if (hitDetection.collider.gameObject.tag == "Rock" && gameObject.tag == "Big Brainless")
                {
                    anim.SetBool("Run", true);

                    positionToMove = transform.position + transform.forward * speedPush * Time.deltaTime;
                    Vector3 hitDetectionPositionToMove = hitDetection.transform.position + transform.forward * speedPush * Time.deltaTime;

                    GetComponent<Rigidbody>().MovePosition(Vector3.Lerp(transform.position, positionToMove, smoothPlayerMove));
                    hitDetection.collider.GetComponent<Rigidbody>().MovePosition(Vector3.Lerp(hitDetection.transform.position, 
                                             hitDetectionPositionToMove, smoothPlayerMove));

                    hitDetection.collider.transform.Rotate(transform.right);
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
                transform.rotation = camZoom.transform.rotation;
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

    private IEnumerator PickUp()
    {
       
            
            if (isPicked == false)
                
            {
                if (/*Input.GetAxis("Fire1") > 0*/ Input.GetButton("Fire3") && isPicked == false)
                {
                    
                    isPicked = true;
                    anim.SetBool("Crouch", true);
                    isAxisF1InUse = true;
                    
                    yield return new WaitForSeconds(.5f);
                
                    otherGameObject.transform.position = hangingObjectPosition.transform.position;

                    otherGameObject.GetComponent<Rigidbody>().isKinematic = true;
                    otherGameObject.GetComponent<Rigidbody>().detectCollisions = false;

                    isPickable = false;
                    yield return new WaitForEndOfFrame();
                    otherGameObject.transform.eulerAngles = otherGameObject.GetComponent<PositionWhenPicked>().position;
                    
                    otherGameObject.transform.parent = hangingObjectPosition.transform;
                    anim.SetBool("Crouch", true);
                    anim.SetBool("Crouch", false);
            }

                else if (/*Input.GetAxisRaw("Fire1") == 0*/ Input.GetButtonUp("Fire3"))
            {
                isAxisF1InUse = false;
            }
                 
        }
          
    }

    void ThrowEvent()

    {
        
        isPicked = false;
        otherGameObject.transform.parent = null;
        otherGameObject.GetComponent<Rigidbody>().isKinematic = false;
        

        otherGameObject.GetComponent<Rigidbody>().AddForce((transform.forward * throwStrengthX) + (transform.up * throwStrengthY));
        throwStrengthX = 0;
        throwStrengthY = 0;

        

        //this.transform.GetComponent<CharacterController>().speed *= 3;


        
        if (otherGameObject.GetComponent<Goo>() != null)
        {
            otherGameObject.GetComponent<Goo>().isGooAble = true;
        }
        timerThrowOffset = Time.time + timerThrow;
        isThrowing = true;
        anim.SetBool("Throw", false);

    }

    private void Sneak()
    {

    }

    private void Throw()
    {
       

            if ( isPicked == true)
        {
            if (Input.GetButtonUp("Fire1") || throwStrengthX >= 500 ||  Input.GetButtonUp("Fire3") /*Input.GetAxisRaw("Fire1") == 0*/)
            {
                if (isAxisF1InUse == true)
                {
                    if (throwStrengthX >= 50)
                    {
                        anim.SetBool("Throw", true);
                        
                    }

                    else if (throwStrengthX < 50)
                    {
                        ThrowEvent();
                    }

                    Debug.Log(throwStrengthX);
                   
                }

                


                //cam.GetComponent<CameraController>().CameraDeZoomFocus();
            }

           

            else if (Input.GetButton("Fire1") || Input.GetButton("Fire3") /*Input.GetAxisRaw("Fire1") == 1*/ )

            {
                throwStrengthX += throwStrengh + Time.deltaTime;
                throwStrengthY += throwHigh + Time.deltaTime;
                if (isAxisF1InUse == false)
                {
                    
                    isAxisF1InUse = true;
                }
                

            }

            if (Input.GetButtonDown("Fire1"))
            {

            }
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
        if (Input.GetButtonDown("X") && MindPower.isMindManipulated == false)
        {
            
            rb.velocity += targetRotationCam.transform.forward * 100 * dodgePower * Time.deltaTime;
            yield return new WaitForSeconds(dodgeTimer);
            rb.velocity = new Vector3 (0,0,0);
            
        }
    }

    
}
