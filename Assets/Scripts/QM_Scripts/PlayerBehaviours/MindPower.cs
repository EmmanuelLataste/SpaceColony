using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.Playables;


public class MindPower : MonoBehaviour {

    [Header("FocusPeriod = Fire2")]
    public ParticleSystem possessionParticles;
    RaycastHit hit;
    public Transform currentHit;

    [Header("MindControlPeriod = Fire1")]
    private float timer;
    public float timerOffset;
    public bool isMindManipulated = false;
    public LineRenderer rayon;

    [Header("Camera")]
    public CinemachineVirtualCamera normalCam;
    public CinemachineVirtualCamera zoomCam;
    private bool isZoom = false;

    private float currentVertical2;
    private float smoothRay;
    public float speedRay;
    public Vector3 ray = new Vector3(0, 0, 1);

    private void Start()
    {
        currentHit = null;
    }

    void Update()
    {
        ControlPower();
        Aim();
        ReticleParticles();
        if (Input.GetAxis("Vertical2") >= currentVertical2 && Input.GetAxis("Vertical2") > 0)
        {
            currentVertical2 = Input.GetAxis("Vertical2");
            ray += new Vector3(0, .1f, 0);
            rayon.SetPosition(1, ray);
        }

        else if (Input.GetAxis("Vertical2") <= currentVertical2 && Input.GetAxis("Vertical2") < 0)
        {
            currentVertical2 = Input.GetAxis("Vertical2");
            //rayon.SetPosition(1, new Vector3(0, currentVertical2 * 13, 1));
            ray += new Vector3(0, -.1f, 0);
            rayon.SetPosition(1, ray);

        }

        else if (isFire2Triggered() == false )
        {
            smoothRay = 0;
            currentVertical2 = 0;
            rayon.SetPosition(1, new Vector3(0, currentVertical2, 1));
        }
        

    }
    // A changer de script 
   

    // Fire 1 : When the player try to Mind Manipulate an ennemy
    void ControlPower()
    {
        // We want to control the ennemy
        // If LT and RT are triggered
        if (isFire1Triggered() == true && isFire2Triggered() == true && isAiming() == true)
        {
            
            // If the timer = 0...
                if (timer == 0 )
                {
                Debug.Log("1.1");

                //... so timer = time ( since the beginning of the game ) + (the time we want to wait * the slowdown factor in "Time Manager " because it's during a bullet time)
                timer = (Time.time + (timerOffset /** timeManager.GetComponent<TimeManager>().slowDownFactor*/));
                    zoomCam.GetComponent<CameraZoomController>().CameraShake(1, 1);
                    
                }

                


                // If time > timer...
                 if (Time.time > timer )
                {
                    Debug.Log("1.2");
                // ... so the bool isMindManipulated = true...
                    
                    timer = 0;
                     // ... we desactivate the script CharacterController, our player can't move
                    //GetComponent<CharacterController>().enabled = false;
                //... and we activate the script EnnemiController of the ennemi touched, we can control the ennemu
                //hit.transform.GetComponent<EnnemiController>().enabled = true;
                    //hit.transform.GetComponent<CombatManager>().enabled = false;
                    hit.transform.GetComponent<EnnemiController>().enabled = true;
                    GetComponent<CharacterController>().enabled = false;
                    currentHit = hit.transform;
                    // ... our mind gauge loose 10 of units.

                    normalCam.GetComponent<CameraController>().Follow(currentHit);
                    zoomCam.GetComponent<CameraZoomController>().CameraShake(0, 0);
                    StartCoroutine(FindObjectOfType<CameraController>().CameraShakeTiming(2, 2, .2f));
                isMindManipulated = true;

            }

        }
        // Else if RT is not triggered
        else if (isAiming() == false && isMindManipulated == false)
        {
            if (timer != 0)
            {
                timer = 0;
                zoomCam.GetComponent<CameraZoomController>().CameraShake(0, 0);

            }


        }

        // We are controlling the ennemy and we want to go back in our player
        // Else if RT is triggered and bool isMindManipulated is true...
        else if (isFire1Triggered() == true && isMindManipulated == true && currentHit.transform != null )
        {
            Debug.Log("3");
            //... so we activate our player movement script, and desactivate ennemy, is MindManipulated is false
            //GetComponent<CharacterController>().enabled = true;
            //currentHit.transform.GetComponent<EnnemiController>().enabled = false;
            //currentHit.transform.GetComponent<CombatManager>().speed = 3;

            //currentHit.transform.GetComponent<CombatManager>().enabled = true;
            currentHit.transform.GetComponent<EnnemiController>().enabled = false;
            GetComponent<CharacterController>().enabled = true;
            normalCam.GetComponent<CameraController>().Follow(transform);
            isMindManipulated = false;
            StartCoroutine(FindObjectOfType<CameraController>().CameraShakeTiming(2, 2, .2f));
            currentHit = null;

        }

        else if (isMindManipulated == true && currentHit == null)
        {
            GetComponent<CharacterController>().enabled = true;
            normalCam.GetComponent<CameraController>().Follow(transform);
            StartCoroutine(FindObjectOfType<CameraController>().CameraShakeTiming(2, 2, .2f));
            currentHit = null;

        }

        if (isFire2Triggered() == true && isAiming() == false)
        {
            possessionParticles.gameObject.SetActive(false);
        }


    }

    // Fire 2 : When the player is aiming the ennemy that he tries to Mind Manipulate
    void Aim()
    {
        // If LT is triggered...
        if (isFire2Triggered() == true)
        {
            if (isMindManipulated == false)
            {
                // ... so we activate the GO " Rayon ", and we can see a crosshair orange pointing in front of us
                rayon.GetComponent<LineRenderer>().enabled = true;
                // ... our mind gauge loose -0.1 unit by frame.
                //timeManager.GetComponent<TimeManager>().SlowMotion();
                if (isZoom == false)
                {
                    normalCam.GetComponent<CameraController>().CameraZoomFocus();
                    isZoom = true;
                }


            }

            else
            {
                // ... so we activate the GO " Rayon "
                rayon.GetComponent<LineRenderer>().enabled = false;
                if (isZoom == true)
                {
                    normalCam.GetComponent<CameraController>().CameraDeZoomFocus();
                    isZoom = false;
                }
               
            }
            
        }
        // Else if LT is not triggered and the cam is behin us.
        else if (isFire2Triggered() == false)
        {
            // ... so we activate the GO " Rayon "
            rayon.GetComponent<LineRenderer>().enabled = false;
            if (isZoom == true)
            {
                normalCam.GetComponent<CameraController>().CameraDeZoomFocus();
                isZoom = false;
            }

        }

    }



    // To Know if an ennemy is touched by the ray which allows the Mind Manipulation
    bool isAiming()
    {
        
        Debug.DrawRay(transform.position, transform.right * 100 + rayon.GetPosition(1) , Color.green);
        // If a ray from our position to the right (Vector 3(1,0,0) * 100), touch an object, this object is stocked in " hit "
        if (Physics.Raycast(transform.position, /*transform.right * 100 + transform.up * Input.GetAxis("Vertical2") * 30*/transform.right * 100 + rayon.GetPosition(1), out hit ))
            {
                // If the object stocked in " hit " has the tag " Ennemy "...
                if (hit.transform.gameObject.layer == 11)
                {
                // ... the function is true...
                return true;
                }

            }

       // ... else it's false
        return false;
    }

    bool isFire1Triggered()
    {
        if (Input.GetAxis("Fire1") > 0)
        {
            return true;
        }

        return false;
    }

    bool isFire2Triggered()
    {
        if (Input.GetAxis("Fire2") > 0)
        {
            
            return true;
           
        }


        return false;
    }


    // Particles of the ennemy whe is focused
    void ReticleParticles()
    {
        if (isAiming() == true && Input.GetAxis("Fire2") > 0)
        {

            possessionParticles.gameObject.transform.position = hit.transform.position;
            possessionParticles.gameObject.SetActive(true);
        }

        else if (isMindManipulated == true && currentHit.transform != null)
        {
            possessionParticles.transform.parent = currentHit.transform;
        }

        else
        {

            possessionParticles.gameObject.SetActive(false);
        }
    }

  

}
