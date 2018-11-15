using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TargetRotation : MonoBehaviour {
    public GameObject cam;
    private float currentRotation;
    private float rotation;
    private float vertical;
    private float horizontal;
    


    void Update () {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        Rotation();
        RotationCam3();
	}

    void Rotation()
    {
        if (horizontal != 0 || vertical != 0)
        {
            //rotation = (Mathf.Atan2(-vertical, horizontal) * Mathf.Rad2Deg);

            //transform.rotation = Quaternion.Euler(new Vector3(0, rotation, 0));
            //transform.rotation = cam.transform.rotation;
        }



      
    }

    void RotationCam3()
    {


        if (Input.GetAxis("Horizontal2") > 0)
        {
            transform.Rotate(Vector3.up * 2f, Space.World);

        }

        else if (Input.GetAxis("Horizontal2") < 0)
        {
            transform.Rotate(-Vector3.up * 2f, Space.World);
        }

    }


}
