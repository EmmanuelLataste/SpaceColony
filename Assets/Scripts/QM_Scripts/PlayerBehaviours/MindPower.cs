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
    public Vector3 ray = new Vector3(0, 0, 0);
    public Vector3 ray2 = new Vector3(0, 0, 1);

    public Vector3 offset;
    public float maxRange;


    private void Start()
    {
        currentHit = null;
    }

    void Update()
    {
        ControlPower();
        Aim();
        ReticleParticles();
        RayonController();
        if ( isMindManipulated == false && hit.collider != null)
        {
            offset = transform.position - hit.transform.position;
        }

        else if (isMindManipulated == true && currentHit != null)
            offset = transform.position - currentHit.transform.position;




    }
    // A changer de script 
   
    void RayonController()
    {
        if (Input.GetAxis("Vertical2") < 0 && rayon.GetPosition(1).y < 20)
        {
            ray += new Vector3(0, .4f, 0);
            ray2 += new Vector3(0, 0.92f, 0);
            rayon.SetPosition(1, ray);
        }

        else if (Input.GetAxis("Vertical2") > 0 && rayon.GetPosition(1).y > -7)
        {
            //rayon.SetPosition(1, new Vector3(0, currentVertical2 * 13, 1));
            ray += new Vector3(0, -.4f, 0);
            ray2 += new Vector3(0, -0.92f, 0);
            rayon.SetPosition(1, ray);

        }

        else if (isFire2Triggered() == false)
        {
            ray = new Vector3(0, 0, 1);
            ray2 = new Vector3(0, 0, 1);
            rayon.SetPosition(1, ray);
        }
    }
    // Fire 1 : When the player try to Mind Manipulate an ennemy
    void ControlPower()
    {
        // We want to control the ennemy
        // If LT and RT are triggered
        if (isFire1Triggered() == true && isFire2Triggered() == true && isAiming() == true &&  offset.x < maxRange && offset.x > -maxRange && offset.y < maxRange && offset.y > -maxRange)
        {
            
            // If the timer = 0...
                if (timer == 0 )
                {
                
                   timer = (Time.time + (timerOffset /** timeManager.GetComponent<TimeManager>().slowDownFactor*/));
                    zoomCam.GetComponent<CameraZoomController>().CameraShake(1, 1);
                    
                }
                
                // If time > timer...
                 if (Time.time > timer )
                {
                
                    timer = 0;
                    hit.transform.GetComponent<EnnemiController>().enabled = true;
                    GetComponent<CharacterController>().enabled = false;
                    currentHit = hit.transform; 
                    normalCam.GetComponent<CameraController>().Follow(currentHit);
                    StartCoroutine(FindObjectOfType<CameraController>().CameraShakeTiming(2, 2, .2f));
                    zoomCam.GetComponent<CameraZoomController>().CameraShake(0, 0);
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
                offset = Vector3.zero;
            }
            
        }
        
        else if (isFire1Triggered() == true && isMindManipulated == true || isMindManipulated == false || offset.x > maxRange || offset.x < -maxRange || offset.y > maxRange || offset.y < -maxRange)
        {
            if (currentHit != null )
            {
                currentHit.transform.GetComponent<EnnemiController>().enabled = false;
                GetComponent<CharacterController>().enabled = true;
                normalCam.GetComponent<CameraController>().Follow(transform);
                isMindManipulated = false;
                StartCoroutine(FindObjectOfType<CameraController>().CameraShakeTiming(2, 2, .2f));
                currentHit = null;
            }


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
        
        Debug.DrawRay(transform.position, (ray2 + (transform.right * 100)), Color.green);
        
        // If a ray from our position to the right (Vector 3(1,0,0) * 100), touch an object, this object is stocked in " hit "
        if (Physics.Raycast(transform.position, (ray2 + (transform.right * 100)), out hit ))
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

        else if (isMindManipulated == true && currentHit.transform != null )
        {
            possessionParticles.gameObject.SetActive(false);
        }

      
    }

  

}
