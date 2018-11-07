using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public LayerMask groundLayer; // Think about adding it into the scene and to create the corresponding layer : groundLayer
    private float horizontal;
    private float vertical;
    public float speed = 5f;
    public float jump = 1;
    public Vector3 groundDistance;

    private void Start()
    {

    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal"); // Stock left joystick's values in two variables (between -1 and 1)
        vertical = Input.GetAxis("Vertical");

        Rotation();
        Movements();
        Jump();

    }

    void Rotation()
    {

        if (horizontal != 0 || vertical != 0) // Si on utilise le joystick
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, Mathf.Atan2(-vertical, horizontal) * Mathf.Rad2Deg, 0));
            // We give to the player a new rotation to his Y, the formula is : ArcTan(-vertical / horizontal)
            // MathF.Rad2Deg Is to give the value in degrees as it is in radians.
        }
    }

    void Movements()
    {
        if (horizontal != 0 || vertical != 0)
        {
            //GetComponent<Rigidbody>().MovePosition(transform.position + transform.right);

            //GetComponent<Rigidbody>().AddForce(transform.right * speed);
            GetComponent<Rigidbody>().velocity = (new Vector3(horizontal, 0, vertical) * speed);
            // Accelerate the player.
            // transform.Translate(new Vector3(speed, 0, 0));
        }
    }

    void Jump()
    {
        if (isGrounded() == true) // If the method beneef is true, so if the ray touches the ground
        {
            if (Input.GetButtonDown("Jump")) // If we press on Jump
            {
                Debug.Log("Jump");
                GetComponent<Rigidbody>().AddForce(new Vector3(0, jump, 0));
                // So we add a force to the Y to jump.
            }

        }

    }

    bool isGrounded() // A methode sending back a boolean.
    {

        RaycastHit hit;
        Debug.DrawRay(transform.position, groundDistance, Color.red); // Allow to see the ray in the scene when it's playing.
        if (Physics.Raycast(transform.position, groundDistance, out hit, 2f, groundLayer))
        // If a ray of 2f from the player position, oriented towards the ground (groundDistance) touches an object having the layer "ground" then ...
        {
            Debug.Log(hit.transform.name);
            return true; //Then we send back True

        }
        return false;
    }
}