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
    public static Transform currentHit;
    public static bool isMindManipulated = false;
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

    bool isF1InUse;
    

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
        if (isMindManipulated == false)
        {
            currentHit = null;
        }


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
            Cursor.visible = true;
        }

        if (currentHit != null &&  currentHit.GetComponent<Life>().isAlive == false )
        {
            currentHit = null;
            isMindManipulated = false;
        }
        Debug.Log(currentHit);

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
        if (isMindManipulated == false )
        {
            CharacterController.anim.SetBool("isPossessing", false);
            if (isFire2Triggered() == true)
            {
                CharacterController.anim.SetBool("isPossessing", true);
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

                    if (Input.GetButtonDown("Fire1") == true || Input.GetAxis("Fire1") > 0)
                    {
                        if (isF1InUse == false)
                        {
                            currentHit = hit.transform;

                            //currentHitSpeed = currentHit.transform.GetComponent<NavMeshAgent>().speed;
                            FindObjectOfType<CameraZoomController>().CameraShake(forceOfShake, forceOfShake);
                            StopCoroutine(TimerBeforePossession());
                            StartCoroutine(TimerBeforePossession());
                            isF1InUse = true;
                        }


                    }

                    if (Input.GetButtonUp("Fire1") == true && isPossessionCoroutineRunning == true || Input.GetAxis("Fire1") == 0)
                    {
                        if (isF1InUse == true)
                        {
                            FindObjectOfType<CameraZoomController>().CameraShake(0, 0);
                            isPossessionCoroutineRunning = false;
                            StopAllCoroutines();
                            isF1InUse = false;
                        }

                    }
                }

                else
                {
                    
                    possessionParticles.gameObject.SetActive(false);
                    FindObjectOfType<CameraZoomController>().CameraShake(0, 0);
                    StopAllCoroutines();
                    isF1InUse = false;
                }
            }

            else 
            {
                possessionParticles.gameObject.SetActive(false);
                if (isZoom == true)
                {
                    CharacterController.anim.SetBool("isPossessing", false);
                    //rayon.GetComponent<LineRenderer>().enabled = false;
                    normalCam.GetComponent<CameraController>().CameraDeZoomFocus();
                    isZoom = false;
                    StopAllCoroutines();
                }
            }
        }

        else if (isMindManipulated == true && currentHit.GetComponent<EnnemiController>().isPicked == false)
        {

            if (onceTrue == false)
            {
            
                if (isZoom == true)
                {
                   // rayon.GetComponent<LineRenderer>().enabled = false;
                    normalCam.GetComponent<CameraController>().CameraDeZoomFocus();
                    isZoom = false;
                }
                isF1InUse = false;
                Possession(0,LayerMask.GetMask("Nothing"), true, false, currentHit, true,1);
                
            }

            else if (onceTrue == true)
            {
                if (Input.GetButtonUp("Fire1") || Input.GetAxis("Fire1") == 0)
                {
                    CharacterController.anim.SetBool("isPossessing", false);
                    isF1InUse = true;
                   
                }

                else if (Input.GetButtonDown("Fire1") || Input.GetAxis("Fire1") == 1)
                {
                    if (isF1InUse == true)
                    {
                        CharacterController.anim.SetBool("isPossessing", false);
                        isF1InUse = false;
                        isMindManipulated = false;
                        
                        Possession(currentHitSpeed, LayerMask.GetMask("Player"), false, true, transform, false, .2f);
                        FindObjectOfType<CameraController>().CameraShake(0f, 0f);
                    }

                }
               
            }
            
            
        }

    }


    void Possession(float speed,LayerMask targetLayer, bool isEnemyControlled, bool isPlayerControlled, Transform follow, bool isonceTrue, float timer)
    {
        if ( currentHit != null)
        {
            currentHit.transform.GetComponent<EnnemiController>().enabled = isEnemyControlled;
            //currentHit.transform.GetComponent<NavMeshAgent>().speed = speed;
            //currentHit.transform.GetComponent<PositionEnemies>().enabled = isPlayerControlled ;
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
        yield return null;
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

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Entity"))
        {
            if (isMindManipulated == false && isFire1Triggered() == true && currentHit == null)
            {
                StartCoroutine(CameraController.cam.GetComponent<CameraController>().CameraShakeTiming(1, 1, .25f));
                CharacterController.anim.SetBool("isPossessing", true);
                currentHit = other.transform;
                isMindManipulated = true;
               
            }
        }
    }

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
        if (Input.GetAxisRaw("Fire2") > 0 || Input.GetButton("Fire2"))
        {

            
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
