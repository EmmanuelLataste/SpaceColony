using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.Playables;


public class MindPower : MonoBehaviour {

    [Header("FocusPeriod = Fire2")]
    public ParticleSystem reticle;
    RaycastHit hit;
    Transform currentHit;

    [Header("MindControlPeriod = Fire1")]
    private float timer;
    public float timerOffset;
    private bool isMindManipulated = false;
    public GameObject rayon;

    [Header("Gauge")]
    public Slider gaugePower;

    [Header("Camera")]
    public CinemachineVirtualCamera normalCam;
    public CinemachineVirtualCamera zoomCam;
    private bool isZoom = false;

    [Header("Time")]
    public GameObject timeManager;

    public GameObject targetCam;

    void Update()
    {
        GainGauge();
        ControlPower();
        Focus();
        ReticleParticles();

    }
    // A changer de script 
   

    // Fire 1 : When the player try to Mind Manipulate an ennemy
    void ControlPower()
    {
        // We want to control the ennemy
        // If LT and RT are triggered
        if (isFire1Triggered() == true && isFire2Triggered() == true && isFocused() == true)
        {
            
            // If the timer = 0...
                if (timer == 0 )
                {
                
                    //... so timer = time ( since the beginning of the game ) + (the time we want to wait * the slowdown factor in "Time Manager " because it's during a bullet time)
                    timer = (Time.time + (timerOffset * timeManager.GetComponent<TimeManager>().slowDownFactor));
                    zoomCam.GetComponent<CameraZoomController>().CameraShake(1, 1);
                    
                }
                // If time > timer...
                if (Time.time > timer)
                {
                    // ... so the bool isMindManipulated = true...
                    isMindManipulated = true;
                    timer = 0;
                    // ... we desactivate the script CharacterController, our player can't move
                    GetComponent<CharacterController>().enabled = false;
                    //... and we activate the script EnnemiController of the ennemi touched, we can control the ennemu
                    hit.transform.GetComponent<EnnemiController>().enabled = true;
                    hit.transform.GetComponent<CombatManager>().speed = 0;
                    currentHit = hit.transform;
                    // ... our mind gauge loose 10 of units.
                    MindGauge(-10f);

                    normalCam.GetComponent<CameraController>().Follow(currentHit);
                    zoomCam.GetComponent<CameraZoomController>().CameraShake(0, 0);
                    StartCoroutine(FindObjectOfType<CameraController>().CameraShakeTiming(2, 2, .2f));


            }

        }
        // Else if RT is not triggered
        else if (isFire1Triggered() == false)
        {
            timer = 0;
            zoomCam.GetComponent<CameraZoomController>().CameraShake(0, 0);

        }

        // We are controlling the ennemy and we want to go back in our player
        // Else if RT is triggered and bool isMindManipulated is true...
        else if (isFire1Triggered() == true && isMindManipulated == true)
        {
            //... so we activate our player movement script, and desactivate ennemy, is MindManipulated is false
            GetComponent<CharacterController>().enabled = true;
            currentHit.transform.GetComponent<EnnemiController>().enabled = false;
            currentHit.transform.GetComponent<CombatManager>().speed = 3;
            normalCam.GetComponent<CameraController>().Follow(transform);
            isMindManipulated = false;
            StartCoroutine(FindObjectOfType<CameraController>().CameraShakeTiming(2, 2, .2f));
            

        }



    }

    // Fire 2 : When the player is aiming the ennemy that he tries to Mind Manipulate
    void Focus()
    {
        // If LT is triggered...
        if (isFire2Triggered() == true)
        {
            if (isMindManipulated == false)
            {
                // ... so we activate the GO " Rayon ", and we can see a crosshair orange pointing in front of us
                rayon.GetComponent<MeshRenderer>().enabled = true;
                // ... our mind gauge loose -0.1 unit by frame.
                MindGauge(-.1f);
                timeManager.GetComponent<TimeManager>().SlowMotion();
                if (isZoom == false)
                {
                    normalCam.GetComponent<CameraController>().CameraZoomFocus();
                    isZoom = true;
                }


            }

            else
            {
                // ... so we activate the GO " Rayon "
                rayon.GetComponent<MeshRenderer>().enabled = false;
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
            rayon.GetComponent<MeshRenderer>().enabled = false;
            if (isZoom == true)
            {
                normalCam.GetComponent<CameraController>().CameraDeZoomFocus();
                isZoom = false;
            }

        }

    }

    // To Know if an ennemy is touched by the ray which allows the Mind Manipulation
    bool isFocused()
    {
        
        Debug.DrawRay(transform.position, transform.right * 100, Color.green);
        // If a ray from our position to the right (Vector 3(1,0,0) * 100), touch an object, this object is stocked in " hit "
        if (Physics.Raycast(transform.position, transform.right * 100, out hit ))
            {
                // If the object stocked in " hit " has the tag " Ennemy "...
                if (hit.transform.CompareTag("Ennemy"))
                {
                Debug.Log(hit.transform);
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


    // Values of the gauge
    public void MindGauge(float gaugeValue)
    {
        gaugePower.value += gaugeValue;
    }

    // Allows the gauge to ride up when the player dont use his power
    void GainGauge()
    {
        if (isMindManipulated == false)
        {
            MindGauge(0.05f);
        }
    }

    // Particles of the ennemy whe is focused
    void ReticleParticles()
    {
        if (isFocused() == true && Input.GetAxis("Fire2") > 0)
        {

            reticle.gameObject.transform.position = hit.transform.position;
            reticle.gameObject.SetActive(true);
        }

        else if (isMindManipulated == true)
        {
            reticle.transform.parent = currentHit.transform;
        }

        else
        {

            reticle.gameObject.SetActive(false);
        }
    }

  

}
