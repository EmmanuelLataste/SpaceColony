﻿using System.Collections;
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
    public float rotationSpeed;

    public GameObject returnToRotationTarget;
    private float smoothReturn;
    public float speedReturn;

    void Update () {
        vertical2 = Input.GetAxis("Vertical2");
        horizontal2 = Input.GetAxis("Horizontal2");
        Rotation();
        StartCoroutine(ReturnBehindPlayer());
        //Rotation2();
    }


   void Rotation2()
    {
        if (horizontal2!= 0 || vertical2 != 0)
        {
            rotation = (Mathf.Atan2(vertical2, horizontal2) * Mathf.Rad2Deg);

            transform.rotation = Quaternion.Euler(new Vector3(0, rotation, 0));
        }

    }

    void Rotation()
    {
        
        if (horizontal2 > 0)
        {
            smoothRotationNegatif = 0;
            transform.Rotate(Vector3.up * Mathf.Lerp(0, rotationSpeed, smoothRotationPositif), Space.World);
            smoothRotationPositif += smoothRotationSpeed * Time.deltaTime;
        }

        else if (horizontal2 < 0)
        {
            smoothRotationPositif = 0;
            transform.Rotate(-Vector3.up * Mathf.Lerp(0, rotationSpeed, smoothRotationNegatif), Space.World);
            smoothRotationNegatif += smoothRotationSpeed * Time.deltaTime;
        }

        else if (horizontal2 == 0)
        {
            smoothRotationNegatif = 0;
            smoothRotationPositif = 0;
        }
        
    }

    private IEnumerator ReturnBehindPlayer()
    {

        if (vertical2 == 0 && horizontal2 == 0 && Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            yield return new WaitForEndOfFrame();
            transform.rotation = Quaternion.Slerp(transform.rotation, returnToRotationTarget.transform.rotation, smoothReturn);
            smoothReturn += speedReturn * Time.deltaTime;
        }

        else
        {
            smoothReturn = 0;
        }
    }

}
