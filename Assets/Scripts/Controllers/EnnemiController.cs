using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiController : MonoBehaviour {

    public LayerMask groundLayer;
    private float horizontal;
    private float vertical;
    public float speed;
    public float jump;
    private float velocityX;
    public Vector3 groundDistance;
    private Quaternion currentRotation;
    private float rotation;
    public float smoothTime;
    private float smoothRotation;

    private void Start()
    {

    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal"); // On stocke les valeurs du joystick gauche dans deux variables ( valeurs entre -1 et 1)
        vertical = Input.GetAxis("Vertical");
        Rotation();
        Movements();
        Jump();
        FindObjectOfType<MindPower>().MindGauge(-.01f);
    }

    void Rotation()
    {

        if (horizontal != 0 || vertical != 0) // Si on utilise le joystick
        {

            rotation = (Mathf.Atan2(-vertical, horizontal) * Mathf.Rad2Deg);

            transform.rotation = Quaternion.Euler(Vector3.Lerp(new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z), new Vector3(0, (rotation), 0), smoothRotation));
            smoothRotation += 20f * Time.deltaTime;
            // On donne au player une nouvelle roation à son Y, la formule c'est : ArcTangente(-vertical / horizontal)
            // MathF.Rad2Deg c'est pour mettre la valeur en degrès car sion elle est en radians.

        }
        else if (horizontal == 0 || vertical == 0)
        {
            smoothRotation = 0;
        }
        

    }

    void Movements()
    {
        if (isGrounded() == true)
        {
            if (horizontal >= -1 || vertical >= -1)
            {

                    GetComponent<Rigidbody>().velocity = (new Vector3(horizontal, 0, vertical) * speed);

                //GetComponent<Rigidbody>().MovePosition(transform.position + transform.right);

                //GetComponent<Rigidbody>().AddForce(transform.right * speed);

                // Speed permet d'accelerer le player.
                // transform.Translate(new Vector3(speed, 0, 0));
            }
            
        }

    }

    void Jump()
    {
        if (isGrounded() == true) // Si la méthode en dessous est vrai, donc si le rayon touche le sol
        {
            if (Input.GetButtonDown("Jump")) // Si on appuit sur Jump
            {

                GetComponent<Rigidbody>().AddForce(new Vector3(horizontal, jump, vertical));
                // Alors on ajout une force sur Y pour sauter.
            }

        }
    
    }

    bool isGrounded() // Une méthode renvoyant un booléan.
    {

        RaycastHit hit;
        Debug.DrawRay(transform.position, groundDistance, Color.red); // Permet de voir le ray dans la scène lorsque c'est lancé.
        if (Physics.Raycast(transform.position, groundDistance, out hit, 2f, groundLayer))
        // Si un rayon de 2f partant la position du player, allant vers le sol( groundDistance) touche un objet ayant le calque " ground "...
        {

            return true; //Alors on renvoit Vrai

        }
        return false;

    }

  


}

