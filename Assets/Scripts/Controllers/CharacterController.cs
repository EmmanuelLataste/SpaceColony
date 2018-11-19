using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterController : MonoBehaviour {
    [Header("Rotation")]
    private float horizontal;
    private float vertical;
    public float rotation;
    public float speedRotation;
    private float smoothRotation;
    private float currentRotation;
    public GameObject targetRotationGO;
    
    [Header("Jump")]
    public float jump;
    public LayerMask groundLayer;
    public float groundDistance;

    [Header("Move")]
    public float speed;
    private float velocityX;
    private float smoothPositionMoving;
    public float speedPositionMoving;
    private float smoothPositionStoping;
    public float speedPositionStoping;

    [Header("Hold")]
    public bool isHolding = false;



    void Update()
    {
        horizontal = Input.GetAxis("Horizontal"); // On stocke les valeurs du joystick gauche dans deux variables ( valeurs entre -1 et 1)
        vertical = Input.GetAxis("Vertical");
        Rotation();
        //Movements();
        MovementsAltrnatives();
        Jump();


    }

    void Rotation()
    {


        if (horizontal != 0 || vertical != 0) // Si on utilise le joystick
        {
            if (Input.GetAxis("Fire2") == 0)
            {
                rotation = (Mathf.Atan2(-vertical, horizontal) * Mathf.Rad2Deg);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationGO.transform.rotation, smoothRotation);
                //transform.rotation = Quaternion.Euler(Vector3.Lerp(new Vector3(transform.rotation.x, currentRotation, transform.rotation.z), new Vector3(0, rotation, 0), smoothRotation));
                //Quaternion targetRotation = Quaternion.LookRotation(new Vector3(transform.rotation.x, rotation, transform.rotation.z), Vector3.up);
                //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothRotation);
                smoothRotation += speedRotation * Time.deltaTime;

            }
            else if (Input.GetAxis("Fire2") > 0)
            {
                if (horizontal > 0)
                {
                    transform.Rotate(Vector3.up, Space.Self);
                }

                else if (horizontal < 0)
                {
                    transform.Rotate(-Vector3.up, Space.Self);
                }

            }



            // On donne au player une nouvelle roation à son Y, la formule c'est : ArcTangente(-vertical / horizontal)
            // MathF.Rad2Deg c'est pour mettre la valeur en degrès car sion elle est en radians.

        }
        else if (horizontal == 0 || vertical == 0)
        {
            currentRotation = rotation;
            smoothRotation = 0;
        }

    }

    void Movements()
    {
        if (isGrounded() == true)
        {
            if (horizontal >= -1 || vertical >= -1)
            {

                if (Input.GetAxis("Fire2") == 0)
                {
                    //GetComponent<Rigidbody>().velocity = Vector3.forward;
                    GetComponent<Rigidbody>().velocity = (new Vector3(horizontal, 0, vertical) * speed);

                }

                else if (Input.GetAxis("Fire2") > 0)
                {
                    GetComponent<Rigidbody>().velocity = (new Vector3(velocityX - velocityX, 0, 0));
                }


                //GetComponent<Rigidbody>().MovePosition(transform.position + transform.right);

                //GetComponent<Rigidbody>().AddForce(transform.right * speed);

                // Speed permet d'accelerer le player.
                // transform.Translate(new Vector3(speed, 0, 0));
            }

        }

    }
    private float currentVertical;
    private float currentHorizontal;
    void MovementsAltrnatives()
    {
        if (horizontal != 0 || vertical != 0)
        {
            if (Input.GetAxis("Fire2") == 0)
            {


                //Vector3 vectorPlayer = transform.InverseTransformPoint(1,0,0);

                smoothPositionStoping = 0;
                transform.Translate(Vector3.Lerp((Vector3.zero), new Vector3(vertical / speed, 0, -horizontal / speed) , smoothPositionMoving));
                currentHorizontal = horizontal;
                currentVertical = vertical;
                smoothPositionMoving += speedPositionMoving * Time.deltaTime;
                //GetComponent<Rigidbody>().velocity = new Vector3(1,0,0) * speed;
                //Debug.Log(GetComponent<Rigidbody>().velocity);

            }

            else if (Input.GetAxis("Fire2") > 0)
            {
               
                GetComponent<Rigidbody>().velocity = (new Vector3(velocityX - velocityX, 0, 0));
            }
        }



       if (horizontal == 0 && vertical == 0)
        {
            Debug.Log(currentVertical);
            Debug.Log(currentHorizontal);
            smoothPositionMoving = 0;

            transform.Translate(Vector3.Lerp( new Vector3(currentVertical / speed, 0, -currentHorizontal / speed), (Vector3.zero),smoothPositionStoping));
            smoothPositionStoping += speedPositionStoping * Time.deltaTime;

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
