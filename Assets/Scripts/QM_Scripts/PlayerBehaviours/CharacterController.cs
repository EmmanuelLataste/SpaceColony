using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterController : MonoBehaviour {
    [Header("Rotation")]
    private float horizontal;
    private float vertical;
    public float rotationAiming;
    public float speedRotationPlayer;
    public float smoothRotationPlayer;
    public GameObject targetRotationCam;

    private float currentRotationY;
    private bool isAimingRotating = false;

    [Header("Jump")]
    [Range(0,20), SerializeField]
    private float jump;
    public LayerMask groundLayer;
    public float groundDistance;


    [Header("Move")]
    public float speed;
    private float smoothPlayerMove;
    public float smoothSpeedPlayerMove;
    private Vector3 positionToMove;
  

    [Header("Hold")]
    public bool isHolding = false;

    [Header("PickObjects")]
    public bool isPickable = false;
    public bool isPicked = false;
    public GameObject hangingObjectPosition;
    [SerializeField]
    private GameObject otherGameObject;
    public GameObject cam;
    private float throwStrengthX;
    public float throwStrengh;
    private float throwStrengthY;
    public float throwHigh;

    private void Start()
    {
        otherGameObject = null;
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal"); // On stocke les valeurs du joystick gauche dans deux variables ( valeurs entre -1 et 1)
        vertical = Input.GetAxis("Vertical");
        Jump();
        Rotation();
        StartCoroutine(PickUp(otherGameObject));
        StartCoroutine(Throw(otherGameObject));

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
                
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationCam.transform.rotation, smoothRotationPlayer);
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
                
                GetComponent<Rigidbody>().AddForce(new Vector3(0, jump * 1000, 0));
                //GetComponent<Rigidbody>().AddForce(new Vector3(horizontal, jump, vertical), ForceMode.Impulse);
                // Alors on ajout une force sur Y pour sauter.
            }
           
        }

    }

    private IEnumerator PickUp(GameObject other)
    {
        if (isPickable == true && isPicked == false)
        {
            if (Input.GetButtonUp("X"))
            {
                other.transform.parent = this.transform;
                other.transform.position = hangingObjectPosition.transform.position;
                other.GetComponent<Rigidbody>().isKinematic = true;
                other.GetComponent<Rigidbody>().detectCollisions = false;
                other.transform.rotation = this.transform.rotation;
                isPickable = false;
                yield return new WaitForEndOfFrame();
                isPicked = true;
            }
        }
    }

    private IEnumerator Throw(GameObject other)
    {
        if (isPicked == true && isPickable == false)
        {
            if (Input.GetButtonUp("X") || throwStrengthX >= 500)
            {
                other.transform.parent = null;
                other.GetComponent<Rigidbody>().isKinematic = false;
                isPicked = false;
                other.GetComponent<Rigidbody>().AddForce((transform.right * throwStrengthX) + (transform.up * throwStrengthY));
                throwStrengthX = 0;
                throwStrengthY = 0;
                yield return new WaitForEndOfFrame();
                //this.transform.GetComponent<CharacterController>().speed *= 3;

                yield return new WaitForSeconds(.01f);
                GetComponent<CharacterController>().speed *= 1.5f;
                other.GetComponent<Rigidbody>().detectCollisions = true;
                //cam.GetComponent<CameraController>().CameraDeZoomFocus();
            }

            if (Input.GetButton("X"))

            {
                throwStrengthX += throwStrengh + Time.deltaTime;
                throwStrengthY += throwHigh + Time.deltaTime;
            }

            if (Input.GetButtonDown("X"))
            {
                GetComponent<CharacterController>().speed /= 1.5f;
                //cam.GetComponent<CameraController>().CameraZoomFocus(1);
                //this.transform.GetComponent<CharacterController>().speed /= 3;
                //this.transform.GetComponent<CharacterController>().speedRotationPlayer /= 3;


            }


        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 13 && isPicked == false)
        {
            otherGameObject = other.gameObject;
            isPickable = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 13)
        {
            isPickable = false;

        }
    }

    bool isGrounded() // Une méthode renvoyant un booléan.
    {

        RaycastHit hit;
        Debug.DrawRay(transform.position, -transform.up * groundDistance, Color.red); // Permet de voir le ray dans la scène lorsque c'est lancé.
        if (Physics.Raycast(transform.position, -transform.up, out hit, groundDistance, groundLayer))
        // Si un rayon de 2f partant la position du player, allant vers le sol( groundDistance) touche un objet ayant le calque " ground "...
        {
            return true; //Alors on renvoit Vrai
        }
        return false;

    }

    

  
}
