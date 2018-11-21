using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotationWithCam : MonoBehaviour {
    [Header("Rotation")]
    private float horizontal;
    private float vertical;
    
    public float speedRotation;
    private float smoothRotation;
    private float currentRotation;
    public GameObject targetRotationGO;

    [SerializeField]
    private float rotation;

    void Update ()
    {
        horizontal = Input.GetAxis("Horizontal"); // On stocke les valeurs du joystick gauche dans deux variables ( valeurs entre -1 et 1)
        vertical = Input.GetAxis("Vertical");
        Rotation();	
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
}
