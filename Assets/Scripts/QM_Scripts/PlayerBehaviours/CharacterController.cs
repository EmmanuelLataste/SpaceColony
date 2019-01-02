using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterController : Flammable {
    [Header("Rotation")]
    float horizontal;
    float vertical;
    public float rotationAiming;
    public float speedRotationPlayer;
    float smoothRotationPlayer;
    public GameObject targetRotationCam;
    bool isAimingRotating = false;

    [Header("Jump")]
    [ SerializeField]
    float jump;
    public float groundDistance;

    [Header("Move")]

    public float speed;
    float smoothPlayerMove;
    public float smoothSpeedPlayerMove;
    Vector3 positionToMove;

    [Header("PickObjects")]
    bool isPickable = false;
    bool isPicked = false;
    public GameObject hangingObjectPosition;
    public GameObject otherGameObject;
    bool isHolding = false;

    [Header("ThrowObjects")]
    float throwStrengthX;
    public float throwStrengh;
    float throwStrengthY;
    public float throwHigh;

    [Header("Animations")]
    Animator anim;
    AnimatorStateInfo animStateInfoCrouch;
    int crouchStateHash = Animator.StringToHash("Crouch Layer.Crouch");

    [Header("Cameras")]
    public GameObject cam;
    public GameObject camZoom;
    public Camera mainCam;


    GameObject player;
    MindPower mindPower;

    public bool isAlive = true;
    
    private void Start()
    {
        anim = GetComponent<Animator>();
        mindPower = GetComponent<MindPower>();
        otherGameObject = null;
        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    void Update()
    {
        
        animStateInfoCrouch = anim.GetCurrentAnimatorStateInfo(1);
        horizontal = Input.GetAxis("Horizontal"); // On stocke les valeurs du joystick gauche dans deux variables ( valeurs entre -1 et 1)
        vertical = Input.GetAxis("Vertical");
        Dead();
        Jump();
        Rotation();
        if (isPickable == true && isPicked == false)
            StartCoroutine(PickUp(otherGameObject));
        if (isPicked == true && isPickable == false && mindPower.isAiming2() == false)
            StartCoroutine(Throw(otherGameObject));
        Debug.DrawRay(transform.position, transform.right, Color.yellow);
        

        anim.SetFloat("Direction", vertical);
    }

    private void FixedUpdate()
    {
        Movements();
    }


    void Rotation()
    {

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            if (Input.GetAxis("Fire2") == 0 || player.GetComponent<MindPower>().isMindManipulated == true)
            {
                
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationCam.transform.rotation, smoothRotationPlayer);
                smoothRotationPlayer += speedRotationPlayer * Time.deltaTime;
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
            smoothRotationPlayer = 0;
        }

       
    }

    void Movements()
    {

        if (Input.GetAxis("Fire2") == 0 || player.GetComponent<MindPower>().isMindManipulated == true)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                anim.SetBool("Run", true);
                positionToMove = transform.position + transform.forward * speed * Time.deltaTime;
                GetComponent<Rigidbody>().MovePosition(Vector3.Lerp(transform.position, positionToMove, smoothPlayerMove));
                smoothPlayerMove += smoothSpeedPlayerMove * Time.deltaTime;
            }

            else
            {
                anim.SetBool("Run", false);
                smoothPlayerMove = 0;
            }
        }

        else if (Input.GetAxis("Fire2") > 0 && player.GetComponent<MindPower>().isMindManipulated == false)
        {
            anim.SetBool("Run", false);
            transform.rotation = camZoom.transform.rotation;
            positionToMove = transform.position;
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

    private IEnumerator PickUp(GameObject other)
    {
       
        
            if (Input.GetButtonUp("B"))
                
            {
                anim.SetBool("Crouch", true);
                //if (animStateInfoCrouch.fullPathHash == crouchStateHash)
                //{
                yield return new WaitForSeconds(0.2f);
                
                    other.transform.position = hangingObjectPosition.transform.position;

                    other.GetComponent<Rigidbody>().isKinematic = true;
                    other.GetComponent<Rigidbody>().detectCollisions = false;

                    isPickable = false;
                    yield return new WaitForEndOfFrame();
                    other.transform.eulerAngles = other.GetComponent<PositionWhenPicked>().position;
                    other.transform.parent = hangingObjectPosition.transform; anim.SetBool("Crouch", true);
                    isPicked = true;
                    anim.SetBool("Crouch", false);
                
                //}
            }
          
        
    }

    private IEnumerator Throw(GameObject other)
    {
        
            if (Input.GetButtonUp("B") || throwStrengthX >= 500)
            {
                isPicked = false;
                anim.SetTrigger("Throw");
                yield return new WaitForSeconds(.65f);
                other.transform.parent = null;
                other.GetComponent<Rigidbody>().isKinematic = false;
                
                other.GetComponent<Rigidbody>().AddForce((transform.forward * throwStrengthX) + (transform.up * throwStrengthY));
                throwStrengthX = 0;
                throwStrengthY = 0;
                yield return new WaitForEndOfFrame();
                //this.transform.GetComponent<CharacterController>().speed *= 3;
                yield return new WaitForSeconds(.01f);
                other.GetComponent<Rigidbody>().detectCollisions = true;
                
                //cam.GetComponent<CameraController>().CameraDeZoomFocus();
            }

            else if (Input.GetButton("B"))

            {
                
                throwStrengthX += throwStrengh + Time.deltaTime;
                throwStrengthY += throwHigh + Time.deltaTime;
              
            }

            if (Input.GetButtonDown("B"))
            {
               
            }
            
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pickable") && isPicked == false)
        {
            otherGameObject = other.gameObject;
            isPickable = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pickable"))
        {
            isPickable = false;

        }
    }

    public bool DetectCollisions(float maxDistance)
    {
        if (Physics.Raycast(transform.position, transform.right, maxDistance)) 
        {
            return true;
        }
        return false;
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

    void Dead()
    {
        if (isAlive == false)
        {
            gameObject.SetActive(false);
            
        }
    }

    
}
