using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationPlayer : MonoBehaviour {
    public GameObject targetCam;
    private  float smoothRotationPlayer;
    public float rotationFocus;
    [Range(0, 15)]
    public float speedRotationPlayer;


    void Update () {
        Rotation();
	}

    void Rotation()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            if (Input.GetAxis("Fire2") == 0)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetCam.transform.rotation, smoothRotationPlayer);
                smoothRotationPlayer += speedRotationPlayer * Time.deltaTime;
            }
             else if (Input.GetAxis("Fire2") > 0)
            {
                if (Input.GetAxis("Horizontal") > 0)
                {
                    transform.Rotate(Vector3.up * rotationFocus * Time.deltaTime, Space.Self);
                }

                else if (Input.GetAxis("Horizontal") < 0)
                {
                    transform.Rotate(-Vector3.up * rotationFocus * Time.deltaTime, Space.Self);
                }

            }

        }
        
        else if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            smoothRotationPlayer = 0;
        }
           
        
    }
}
