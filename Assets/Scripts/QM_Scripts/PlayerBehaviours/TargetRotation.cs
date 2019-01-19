using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TargetRotation : MonoBehaviour {
    public GameObject cam;
    private float currentRotation;
    private float rotation;
    private float vertical2;
    private float horizontal2;
    public float smoothRotationNegatif;
    public float smoothRotationPositif;
    float smoothRotationSpeed;
    float rotationSpeed;

    private float smoothReturn;

    CameraController cameraController;

    float speedMouseX;
    float speedMouseY;

    private float initialMouseX = 0;
    private float initialMouseY = 0;

    private float initialHorizontal;
    private float initialVertical;

    public GameObject player;

    private bool isAxisF2inUse;
    float timerF2;
    public float timerF2Offset;

    private void Start()
    {
        cameraController = cam.GetComponent<CameraController>();
        speedMouseX = cameraController.speedMouseX;
        speedMouseY = cameraController.speedMouseY;
        smoothRotationSpeed = cameraController.smoothRotationSpeed;
        rotationSpeed = cameraController.rotationSpeed;
    }
    void Update () {
        vertical2 = Input.GetAxis("Vertical2");
        horizontal2 = Input.GetAxis("Horizontal2");
        Rotation();
        //StartCoroutine(ReturnBehindPlayer());
        //Rotation2();
        CameraMouse();
        
    }


    void CameraMouse()
    {
        if (Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0)
        {
            initialMouseX += speedMouseX * Input.GetAxis("Mouse X");

            if (Input.GetButton("Fire2") == false)
            {
                transform.eulerAngles = new Vector3(0, initialMouseX, 0);

            }

            if (Input.GetButtonUp("Fire2"))
            {
                player.transform.eulerAngles = new Vector3(0, initialMouseX, 0);

            }
            if (Input.GetButton("Fire2") == true)
            {
                if (transform.rotation.eulerAngles.x <= 40 && transform.rotation.eulerAngles.x >= 0)
                {

                    initialMouseY -= speedMouseY * Input.GetAxis("Mouse Y");
                }


                else if (transform.rotation.eulerAngles.x >= 320 && transform.rotation.eulerAngles.x < 360 || transform.rotation.eulerAngles.x < 0)
                {
                    initialMouseY -= speedMouseY * Input.GetAxis("Mouse Y");

                }

                else if (transform.rotation.eulerAngles.x > 40 && transform.rotation.eulerAngles.x < 200 && Input.GetAxis("Mouse Y") > 0)
                {
                    initialMouseY -= speedMouseY * Input.GetAxis("Mouse Y");
                }


                else if (transform.rotation.eulerAngles.x < 320 && transform.rotation.eulerAngles.x > 250 && Input.GetAxis("Mouse Y") < 0)
                {

                    initialMouseY -= speedMouseY * Input.GetAxis("Mouse Y");

                }

                transform.eulerAngles = new Vector3(initialMouseY, initialMouseX, 0);

            }

            
        }
        // Manette
        
            if (Input.GetAxis("Fire2") == 0 || MindPower.isMindManipulated == true)
            {
                transform.eulerAngles = new Vector3(0, cam.transform.eulerAngles.y, 0);
                initialHorizontal += speedMouseX * Input.GetAxis("Horizontal2");
                if (isAxisF2inUse == true)
                {
                    initialVertical = 0;
                    player.transform.eulerAngles = new Vector3(0, initialHorizontal, 0);
                    isAxisF2inUse = false;
                }
                
            }
        

         if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            if (Input.GetAxisRaw("Fire2") == 1 && MindPower.isMindManipulated == false)
            {
                if (isAxisF2inUse == false)
                {
                    timerF2 += Time.time + timerF2Offset;
                    isAxisF2inUse = true;
                }

                if (Time.time >= timerF2)
                {
                    if (transform.rotation.eulerAngles.x <= 40 && transform.rotation.eulerAngles.x >= 0)
                    {

                        initialVertical -= speedMouseY * Input.GetAxis("Vertical");
                    }


                    else if (transform.rotation.eulerAngles.x >= 320 && transform.rotation.eulerAngles.x < 360 || transform.rotation.eulerAngles.x < 0)
                    {
                        initialVertical -= speedMouseY * Input.GetAxis("Vertical");
                    }

                    else if (transform.rotation.eulerAngles.x > 40 && transform.rotation.eulerAngles.x < 200 && Input.GetAxis("Vertical") > 0)
                    {
                        initialVertical -= speedMouseY * Input.GetAxis("Vertical");
                    }

                    else if (transform.rotation.eulerAngles.x < 320 && transform.rotation.eulerAngles.x > 250 && Input.GetAxis("Vertical") < 0)
                    {

                        initialVertical -= speedMouseY * Input.GetAxis("Vertical");
                    }

                    initialHorizontal += speedMouseX * Input.GetAxis("Horizontal");
                    transform.eulerAngles = new Vector3(initialVertical, initialHorizontal, 0);
                    timerF2 = 0;

                }

            }

        }


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
        if (Input.GetAxis("Mouse Y") == 0 && Input.GetAxis("Mouse X") == 0)
        {
            if (horizontal2 > 0)
            {
                //smoothRotationNegatif = 0;
                transform.Rotate(Vector3.up * Mathf.Lerp(0, rotationSpeed, smoothRotationPositif), Space.World);
                //smoothRotationPositif += smoothRotationSpeed * Time.deltaTime;
            }

            else if (horizontal2 < 0)
            {
                //smoothRotationPositif = 0;
                transform.Rotate(-Vector3.up * Mathf.Lerp(0, rotationSpeed, smoothRotationNegatif), Space.World);
                //smoothRotationNegatif += smoothRotationSpeed * Time.deltaTime;
            }

            else if (horizontal2 == 0)
            {
                //smoothRotationNegatif = 0;
                //smoothRotationPositif = 0;
            }
        }
          
        
    }

    //private IEnumerator ReturnBehindPlayer()
    //{

    //    if (vertical2 == 0 && horizontal2 == 0 && Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
    //    {
    //        yield return new WaitForEndOfFrame();
    //        transform.rotation = Quaternion.Slerp(transform.rotation, returnToRotationTarget.transform.rotation, smoothReturn);
    //        smoothReturn += speedReturn * Time.deltaTime;
    //    }

    //    else
    //    {
    //        smoothReturn = 0;
    //    }
    //}

}
