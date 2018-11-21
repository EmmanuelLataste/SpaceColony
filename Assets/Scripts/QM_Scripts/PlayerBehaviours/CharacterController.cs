using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterController : MonoBehaviour {
    [Header("Rotation")]
    private float horizontal;
    private float vertical;
    public float rotationAiming;
    public float speedRotationPlayer;
    private float smoothRotationPlayer;
    public GameObject targetCam;

    private float currentRotationY;
    private bool isAimingRotating = false;

    [Header("Jump")]
    public float jump;
    public LayerMask groundLayer;
    public float groundDistance;

    [Header("Move")]
    public float speed;
    private float smoothPlayerMove;
    public float smoothSpeedPlayerMove;
    private Vector3 positionToMove;
  

    [Header("Hold")]
    public bool isHolding = false;

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal"); // On stocke les valeurs du joystick gauche dans deux variables ( valeurs entre -1 et 1)
        vertical = Input.GetAxis("Vertical");
        Jump();
        Rotation();
       
    }

    private void FixedUpdate()
    {
        Movements();
    }


    void Rotation()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            if (Input.GetAxis("Fire2") == 0)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetCam.transform.rotation, smoothRotationPlayer);
                smoothRotationPlayer += speedRotationPlayer * Time.deltaTime;
                isAimingRotating = false;
            }
            else if (Input.GetAxis("Fire2") > 0)
            {
              
                 if (Input.GetAxis("Horizontal") > 0)
                 {
                     
                       transform.Rotate(Vector3.up * rotationAiming * Time.deltaTime, Space.Self);
                     
                 }

                 else if (Input.GetAxis("Horizontal") < 0)
                 {
                    
                        transform.Rotate(-Vector3.up * rotationAiming * Time.deltaTime, Space.Self);
                    
                 }

                 if (Input.GetAxis("Vertical2") > 0)
                {
                    transform.Rotate(Vector3.up * rotationAiming * Time.deltaTime, Space.Self);
                }


            }

        }

        else if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            smoothRotationPlayer = 0;
        }
    }

    void Movements()
    {

        if (Input.GetAxis("Fire2") == 0)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                positionToMove = transform.position + transform.right * speed * Time.deltaTime;
                GetComponent<Rigidbody>().MovePosition(Vector3.Lerp(transform.position, positionToMove, smoothPlayerMove));
                smoothPlayerMove += smoothSpeedPlayerMove * Time.deltaTime;
            }

            else
            {
                smoothPlayerMove = 0;
            }
        }

        else if (Input.GetAxis("Fire2") > 0)
        {
            positionToMove = transform.position;
        }
            
                
    }
    
    void Jump()
    {
        if (isGrounded() == true) // Si la méthode en dessous est vrai, donc si le rayon touche le sol
        {
            if (Input.GetButtonDown("Jump")) // Si on appuit sur Jump
            {
                
                GetComponent<Rigidbody>().velocity = new Vector3(horizontal, jump, vertical);
                //GetComponent<Rigidbody>().AddForce(new Vector3(horizontal, jump, vertical), ForceMode.Impulse);
                // Alors on ajout une force sur Y pour sauter.
            }
           
        }

    }
    
    bool isGrounded() // Une méthode renvoyant un booléan.
    {
        
        RaycastHit hit;
        Debug.DrawRay(transform.position, -transform.up, Color.red); // Permet de voir le ray dans la scène lorsque c'est lancé.
        if (Physics.Raycast(transform.position, - transform.up, out hit, groundDistance, groundLayer))
            // Si un rayon de 2f partant la position du player, allant vers le sol( groundDistance) touche un objet ayant le calque " ground "...
        {
            
            return true; //Alors on renvoit Vrai
            
        }
        return false;
       
    }

    

  
}
