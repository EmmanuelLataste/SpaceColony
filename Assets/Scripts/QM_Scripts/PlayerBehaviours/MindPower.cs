using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.Playables;


public class MindPower : MonoBehaviour {

    [Header("Control")]
    public ParticleSystem possessionParticles;
    RaycastHit hit;
    public Transform currentHit;
    public bool isMindManipulated = false;
    public LineRenderer rayon;
    public bool onceTrue = false;
    private bool isPossessionCoroutineRunning = false;
    public float timerPossess = 2;
    [Range(0,5)]
    public float forceOfShake;
    float currentHitSpeed;
    

    [Header("Camera")]
    public Camera cameraMain;
    public CinemachineVirtualCamera normalCam;
    public CinemachineVirtualCamera zoomCam;
    private bool isZoom = false;

    [Header("Ray")]
    private Vector3 ray = new Vector3(0, 0, 0);
    private Vector3 ray2 = new Vector3(0, 0, 1);
    private Vector3 offset;
    public float maxRange;
 




    private void Start()
    {
        currentHit = null;
    }

    void Update()
    {
        Control();
        currentHitNull();
        //ReticleParticles();
        //RayonController();

        if ( isMindManipulated == false && hit.collider != null)
        {
            offset = transform.position - hit.transform.position;
        }

        else if (isMindManipulated == true && currentHit != null)
            offset = transform.position - currentHit.transform.position;

        if (isFire2Triggered() == false || isMindManipulated == true )
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        else
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if (currentHit != null &&  currentHit.GetComponent<EnnemiController>().isAlive == false )
        {
            currentHit = null;
        }



    }
   
    void RayonController()
    {
        if (Input.GetAxis("Vertical2") < 0 && rayon.GetPosition(1).y < 20)
        {
            ray += new Vector3(0, .2f, 0);
            ray2 += new Vector3(0, 0.695f, 0);
            rayon.SetPosition(1, ray);
        }

        else if (Input.GetAxis("Vertical2") > 0 && rayon.GetPosition(1).y > -7)
        {
            //rayon.SetPosition(1, new Vector3(0, currentVertical2 * 13, 1));
            ray += new Vector3(0, -.2f, 0);
            ray2 += new Vector3(0, -0.695f, 0);
            rayon.SetPosition(1, ray);

        }

        else if (isFire2Triggered() == false)
        {
            ray = new Vector3(0, 0, 1);
            ray2 = new Vector3(0, 0, 1);
            rayon.SetPosition(1, ray);
        }
    }

    void Control()
    {
        if (isMindManipulated == false)
        {
            if (isFire2Triggered() == true)
            {
                //rayon.GetComponent<LineRenderer>().enabled = true;
                if (isZoom == false)
                {
                    normalCam.GetComponent<CameraController>().CameraZoomFocus();
                    isZoom = true;
                }

                if (/*isAiming()==true ||*/ isAiming2() == true)
                {
                    possessionParticles.gameObject.SetActive(true);
                    possessionParticles.gameObject.transform.position = hit.transform.position;
                    possessionParticles.gameObject.transform.parent = hit.transform;

                    if (Input.GetButtonDown("Fire1") == true )
                    {

                        currentHit = hit.transform;
                        currentHitSpeed = currentHit.transform.GetComponent<NavMeshAgent>().speed;
                        FindObjectOfType<CameraZoomController>().CameraShake(forceOfShake, forceOfShake);
                        StopCoroutine(TimerBeforePossession());
                        StartCoroutine(TimerBeforePossession());

                    }

                    if (isFire1Triggered() == false && isPossessionCoroutineRunning == true)
                    {
                        FindObjectOfType<CameraZoomController>().CameraShake(0, 0);
                        isPossessionCoroutineRunning = false;
                        StopAllCoroutines();
                    }
                }

                else
                {
                    possessionParticles.gameObject.SetActive(false);
                    FindObjectOfType<CameraZoomController>().CameraShake(0, 0);
                    StopAllCoroutines();
                }
            }

            else 
            {
                possessionParticles.gameObject.SetActive(false);
                if (isZoom == true)
                {
                    
                    //rayon.GetComponent<LineRenderer>().enabled = false;
                    normalCam.GetComponent<CameraController>().CameraDeZoomFocus();
                    isZoom = false;
                }
            }
        }

        else if (isMindManipulated == true)
        {

            if (onceTrue == false)
            {
            
                if (isZoom == true)
                {
                   // rayon.GetComponent<LineRenderer>().enabled = false;
                    normalCam.GetComponent<CameraController>().CameraDeZoomFocus();
                    isZoom = false;
                }
                Possession(0,LayerMask.GetMask("Nothing"), true, false, currentHit, true,1);
                
            }

            else if (Input.GetButtonDown("Fire1") && onceTrue == true)
            {
                Possession(currentHitSpeed, LayerMask.GetMask("Player"),false, true,transform, false,.2f);
                isMindManipulated = false;
            }

          
            
        }

    }

    void Possession(float speed,LayerMask targetLayer, bool isEnemyControlled, bool isPlayerControlled, Transform follow, bool isonceTrue, float timer)
    {
        if ( currentHit != null)
        {
            currentHit.transform.GetComponent<EnnemiController>().enabled = isEnemyControlled;
            currentHit.transform.GetComponent<NavMeshAgent>().speed = speed;
            StartCoroutine(FindObjectOfType<CameraController>().CameraShakeTiming(2, 2, .2f));
        }

        GetComponent<CharacterController>().enabled = isPlayerControlled;

        normalCam.GetComponent<CameraController>().Follow(follow);
        zoomCam.GetComponent<CameraZoomController>().CameraShake(0, 0);

        onceTrue = isonceTrue;
    }

    IEnumerator TimerBeforePossession()
    {
        isPossessionCoroutineRunning = true;
        yield return new WaitForSeconds(timerPossess);
        isMindManipulated = true;
    }

    // To Know if an ennemy is touched by the ray which allows the Mind Manipulation
    //bool isAiming()
    //{


    //    Debug.DrawRay(transform.position, (ray2 + (transform.forward * 100)), Color.green);
        
    //    // If a ray from our position to the right (Vector 3(1,0,0) * 100), touch an object, this object is stocked in " hit "
    //    if (Physics.Raycast(transform.position, (ray2 + (transform.forward * 100)), out hit ))
    //        {
    //            // If the object stocked in " hit " has the tag " Ennemy "...
    //            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Entity") )
    //            {
    //            // ... the function is true...
    //            return true;
                
    //            }

                

    //        }

    //   // ... else it's false
    //    return false;
    //}

   public bool isAiming2()
    {
        if (Physics.Raycast(cameraMain.ScreenPointToRay(Input.mousePosition), out hit))

            
         {

            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Entity"))
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
        if (Input.GetAxis("Fire1") > 0 || Input.GetButton("Fire1"))
        {
            return true;
        }

        return false;
    }

    bool isFire2Triggered()
    {
        if (Input.GetAxis("Fire2") > 0 || Input.GetButton("Fire2"))
        {
            
            Cursor.lockState = CursorLockMode.None;
            return true;
          
        }
        



        return false;
    }


    // Particles of the ennemy whe is focused
    void ReticleParticles()
    {

        if (/*isAiming() == true && */Input.GetAxis("Fire2") > 0)
        {

            possessionParticles.gameObject.transform.position = hit.transform.position;
            possessionParticles.gameObject.SetActive(true);
        }

        else if (isMindManipulated == true && currentHit.transform != null )
        {
            possessionParticles.gameObject.SetActive(false);
        }

      
    }

    void currentHitNull()
    {
        if ( currentHit == null)
        {
            Possession(currentHitSpeed, LayerMask.GetMask("Player"), false, true, transform, false, .2f);
            isMindManipulated = false;
        }
    }

  

}
