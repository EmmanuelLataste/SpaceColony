using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TargetRotation : MonoBehaviour {
    public GameObject cam;
    private float currentRotation;
    private float rotation;
    private float vertical2;
    private float horizontal2;
    private float smoothRotationNegatif;
    private float smoothRotationPositif;
    public float smoothRotationSpeed;




    void Update () {
        vertical2 = Input.GetAxis("Vertical2");
        horizontal2 = Input.GetAxis("Horizontal2");
        Rotation();
        //Rotation2();
        RotationCam3();
	}

    void Rotation()
    {
        //if (horizontal != 0 || vertical != 0)
        //{
        //    //rotation = (Mathf.Atan2(-vertical, horizontal) * Mathf.Rad2Deg);

        //    //transform.rotation = Quaternion.Euler(new Vector3(0, rotation, 0));
        //    //transform.rotation = cam.transform.rotation;
        //}





      
    }

    void Rotation2()
    {
        if (horizontal2 != 0 || vertical2 != 0)
        {
            rotation = (Mathf.Atan2(-vertical2, horizontal2) * Mathf.Rad2Deg);

            transform.rotation = Quaternion.Euler(new Vector3(0, rotation , 0));
        }
        
    }

    void RotationCam3()
    {


        if (horizontal2 > 0)
        {
            smoothRotationNegatif = 0;
            transform.Rotate(Vector3.up * Mathf.Lerp(0, 2f, smoothRotationPositif), Space.World);
            smoothRotationPositif += smoothRotationSpeed * Time.deltaTime;

        }

        else if (horizontal2 < 0)
        {
            smoothRotationPositif = 0;
            transform.Rotate(-Vector3.up * Mathf.Lerp(0, 2f, smoothRotationNegatif), Space.World);
            smoothRotationNegatif += smoothRotationSpeed * Time.deltaTime;
        }

        else if (horizontal2 == 0)
        {
            smoothRotationNegatif = 0;
            smoothRotationPositif = 0;
        }

    }


}
