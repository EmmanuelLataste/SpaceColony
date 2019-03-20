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
    public static float timerPossess = 2;
    [SerializeField] float timerPosseIns;
    public static float radiusContactControl = .5f;
    [SerializeField] float radiusContactControlIns;
    public static float rangeManip = 15;
    [SerializeField] float rangeManipIns;
    float currentHitSpeed;
    private bool isZoom = false;
    CharacterController cc;
    CharacterController controledcc;
    [SerializeField] public Transform followPlayer;
    [SerializeField] LayerMask ignoreCollider;
    static Transform hitControlled;
    Animator anim;
    Collider[] contactControl;
    Rippleeffect re;
    public float distance;
    [SerializeField] GameObject playerLayerAI;
    [SerializeField] LayerMask possessMask;
    Transform currentHitPositionTransform;
    bool hitCanBeManipulated;
    bool currentHitisAlive;
    bool soFar;
    bool isContactControl;
    bool isAlive;
    bool canMindM = true;
    bool isF1InUse;


    [Header("Camera")]
    public Camera cameraMain;
    public CinemachineVirtualCamera normalCam;
    public CinemachineVirtualCamera zoomCam;
    [SerializeField] TargetRotation targetRotation;
    GameObject normalCamGO;
    GameObject zoomCamGO;
    CameraZoomController camZC;
    CameraController camC;
    [SerializeField] public CinemachineVirtualCamera camControl;

    [Header("FeedBacks")]
    [SerializeField] public Collider[] rangeFeedback;
    GameObject possParticles;
    public Collider[] rangeFeedbackMemories;
    [SerializeField] GameObject lineMM;
    LineRenderer lineRendererMM;
    [Range(0, 5)]
    public float forceOfShake;
    bool isPossessionParticles;
    bool onceRangeFeedback;


    private void Start()
    {

        lineRendererMM = lineMM.GetComponent<LineRenderer>();
        re = cameraMain.GetComponent<Rippleeffect>();
        normalCamGO = normalCam.gameObject;
        zoomCamGO = zoomCam.gameObject;
        anim = GetComponent<Animator>();
        camC = normalCam.GetComponent<CameraController>();
        camZC = zoomCam.GetComponent<CameraZoomController>();
        cc = GetComponent<CharacterController>();

        currentHit = null;
        hitControlled = transform;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.black;
    //    Gizmos.DrawRay(cameraMain.ScreenPointToRay(Input.mousePosition * 100));
    //}

    void Update()
    {
        rangeManipIns = rangeManip;
        timerPosseIns = timerPossess;
        radiusContactControlIns = radiusContactControl;
        
        isAiming2();
        Control();
        ContactControl();
        DiedTransfer();

        if (currentHit != null)
        {
            
            currentHitisAlive = currentHit.GetComponent<Life>().isAlive;
            camControl.m_Follow = currentHit.Find("PositionWhenPicked").transform;
            if (isMindManipulated == true)
            {
                lineMM.SetActive(true);
                lineRendererMM.SetPosition(0, transform.position );
                lineRendererMM.SetPosition(1, currentHit.transform.position + new Vector3(0, 1.5f, 0));
                lineRendererMM.startWidth = 1 /distance * 2;
                lineRendererMM.endWidth = 1 /distance * 2;
            }


            distance = Vector3.Distance(transform.position, currentHit.transform.position);
            if (distance > rangeManip + 25)
            {
                Debug.Log("WTF");
                soFar = true;
                distance = 0;
                camC.CameraShake(0, 0);
                Transfer(false, true, followPlayer);
                isMindManipulated = false;
                
            }

            else if (distance < rangeManip ) soFar = false;


        }

        

        if (isMindManipulated == false)
        {
            lineMM.SetActive(false);

        }


        if (isMindManipulated == true)
        {
            anim.SetBool("Run", false);
            hitCanBeManipulated = false;
        }

        else
        {
            lineMM.SetActive(false);
        }
        ShakeDistance();
        RangeFeedback();
        
    }

    void RangeFeedback()
    {
        rangeFeedback = Physics.OverlapSphere(transform.position, rangeManip - 2.9f, possessMask);
        if (rangeFeedbackMemories != rangeFeedback)
        {
            foreach(Collider collid in rangeFeedbackMemories)
            {
                collid.GetComponent<CharacterController>().inMMRange = false;
            }

            rangeFeedbackMemories = rangeFeedback;
            onceRangeFeedback = true;
            
        }

        if (onceRangeFeedback == true)
        {
            foreach (Collider collid in rangeFeedback)
            {
                collid.GetComponent<CharacterController>().inMMRange = true;
            }
            onceRangeFeedback = false;
        }


        
    }

    float timerRipple = 0;
    void RippleEffect()
    {

        timerRipple += Time.deltaTime * 2;
        re.RippleEff(transform, timerRipple, .9f);
    }

    float shakeUpgrade;
    void ShakeDistance()
    {
        if (distance >= rangeManip + 20 && isMindManipulated == true)
        {
            camC.CameraShake(.015f * distance, 1f);

        }
    }

   void DiedTransfer()
    {   if (controledcc != null) currentHitisAlive = controledcc.gameObject.GetComponent<Life>().isAlive;

        if (currentHitisAlive == false && canMindM == false)
        {
            StartCoroutine(TransferWhenMMDied());
        }
    }


    IEnumerator TransferWhenMMDied()
    {
        anim.Rebind();
        isMindManipulated = false;
        Destroy(possParticles);
        yield return new WaitForSeconds(2);
        Transfer(false, true, followPlayer);

        //currentHit = null;
        Debug.Log("WTF");
        
        //anim.SetBool("isPossessing", false);
        //camC.Follow(followPlayer);
        //camControl.m_Priority = 8; 
        //cc.isControlled = true;
        yield return null;
    }

    void Control()
    {
        //if (anim.GetCurrentAnimatorStateInfo(2).IsName("Empty") == false && anim.GetCurrentAnimatorStateInfo(2).IsName("Throw") == false)
        //{
        //    cc.canMove = false;
        //}

        //else cc.canMove = true;

        if (isFire1Triggered() == true || soFar == true )
        {
            if (isMindManipulated == true && canMindM == true && currentHit != null)
            {
              
                Transfer(true, false, currentHit.Find("PositionWhenPicked").transform);
            }
               
            else if (isMindManipulated == false && canMindM == false)
            {
                //currentHit = null;
                Transfer(false, true, followPlayer);
            }
    

        }


        Fire1();
        Fire2();

    }

    

    public void ContactControlEvent()
    {
        isContactControl = true;
        //anim.SetTrigger("EndPossess");
        //anim.SetBool("isPossessing", false);
        if (isFire2Triggered() == false && contactControl != null && contactControl.Length != 0)
        {
            
            foreach (Collider contactCol in contactControl)
            {
                if (contactCol.gameObject.layer == LayerMask.NameToLayer("Entity") && contactCol.gameObject.CompareTag("Player") == false && isF1InUse == false)
                {
                    isF1InUse = true;
                    currentHit = contactCol.transform;
                    isMindManipulated = true;
                    controledcc = contactCol.GetComponent<CharacterController>();

                }
            }
        }
    }

    void ContactControl()
    {
        if (isFire1Triggered() == true && isFire2Triggered() == false && isMindManipulated == false)
        {
            contactControl = Physics.OverlapSphere(transform.position, radiusContactControl);
            if (isContactControl == false) /*anim.SetBool("isPossessing", true);*/anim.SetTrigger("StartPossess");

        }
    }

    bool lockAim;
    Vector3 lookPos;
    Quaternion rotation;
    public void isAiming2()
    {
        if (Physics.Raycast(cameraMain.ScreenPointToRay(Input.mousePosition), out hit, rangeManip, possessMask))


        {

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Entity") && hit.collider.gameObject.CompareTag("Player") == false)
            {
                hitCanBeManipulated = hit.collider.GetComponent<CharacterController>().canBeManipulated;
                if (isMindManipulated == false && isFire2Triggered() == true && hitCanBeManipulated == true)

                {
                    if (isFire1Triggered() == true && soFar == false)
                    {

                        camZC.CameraShake(forceOfShake, forceOfShake);
                    }


                    
                    currentHit = hit.transform;
                    controledcc = currentHit.GetComponent<CharacterController>();

                    if (Input.GetAxis("Mouse Y") < 1f && Input.GetAxis("Mouse X") < 1f && lockAim == false)
                    {
                        if (isFire1Triggered() == true)
                        {
                            currentHitPositionTransform = currentHit.Find("PositionWhenPicked").transform;
                            normalCam.gameObject.GetComponent<CameraController>().speedMouseX = 0;
                            normalCam.gameObject.GetComponent<CameraController>().speedMouseY = 0;
                            zoomCam.gameObject.transform.LookAt(currentHitPositionTransform);
                        }
                     

                    }

                    else
                    {
                        lockAim = true;
                        normalCam.gameObject.GetComponent<CameraController>().speedMouseX = 1;
                        normalCam.gameObject.GetComponent<CameraController>().speedMouseY = 1;
                    }

                }

            }
            else
            {
                if (isMindManipulated == false)
                {

                    Debug.Log("fjdi");
                    currentHit = null;
                    camZC.CameraShake(0, 0);


                }

            }


        }

        else
        {
            lockAim = false;
            currentHit = null;

        } 




        // ... else it's false

    }



    bool isFire1Triggered()
    {
        if (Input.GetAxis("Fire1") > 0 || Input.GetButton("Fire1"))
        {
            

            return true;
        }

        else
        {

            camZC.CameraShake(0, 0);
        }
            
        return false;
    }

    public void  Transfer(bool enemyControlled, bool playerControlled, Transform followTransform)
    {
        if (controledcc != null)
        {
            controledcc.isControlled = enemyControlled;
            controledcc.GetComponent<Rigidbody>().isKinematic = playerControlled;
            PositionEnemies pe = controledcc.GetComponent<PositionEnemies>();
            FieldOfView fov = pe.transformPosition.GetComponent<FieldOfView>();
            fov.enabled = playerControlled;
            fov.visible = enemyControlled;
            fov.target = null;
            fov.targetsInViewRadius = null;
            fov.visibleTargets.Clear();

            
            pe.transformPosition.GetComponent<EntityAI>().enabled = playerControlled;
            pe.transformPosition.GetComponent<Animator>().enabled = playerControlled;
            controledcc.GetComponent<Animator>().SetBool("AI", playerControlled);
            controledcc.GetComponent<Animator>().SetBool("Run", false);

            controledcc.GetComponent<Rigidbody>().velocity = Vector3.zero;
            if (playerControlled == true)
            {
                
                if (pe.transformPosition.GetComponent<EntityAI>().waypoints == null)
                {
                    pe.transformPosition.GetComponent<EntityAI>().waypoints[0] = transform;
                }
            }

            else pe.transformPosition.GetComponent<Animator>().Rebind();

        }

    
        anim.SetBool("isPossessing", enemyControlled);
        camC.Follow(followTransform);
        
        targetRotation.enabled = true;
        normalCam.gameObject.SetActive(true);
        if (playerControlled == true)
        {
            camControl.m_Priority = 8;
            
            if (controledcc != null)
            {

                controledcc.isControlChangeColor = false;
                controledcc.canChangeColor = false;
                Destroy(controledcc.gameObject.GetComponent<AudioListener>());
                gameObject.AddComponent<AudioListener>();
            }
                

         
        }
        else
        {
            camControl.m_Priority = 20;
            controledcc.isControlChangeColor = true;
            controledcc.canChangeColor = true;


            Destroy(GetComponent<AudioListener>());
            currentHit.gameObject.AddComponent<AudioListener>();
        }


        //zoomCam.enabled = playerControlled;
        cc.isControlled = playerControlled;


     
        StartCoroutine(camC.CameraShakeTiming(forceOfShake / 2, forceOfShake / 2, .25f));
        canMindM = playerControlled;
        re.RippleEff(transform, 10, .9f);
    

    }


    float timer;
    void Fire1()
    {

        if (isFire1Triggered() == true && isMindManipulated == false && soFar == false)
        {
            

            if (isFire2Triggered() == true && currentHit != null)
            {
                RippleEffect();
                if ( isF1InUse == false)
                {
  
                    timer = Time.time + timerPossess;
                    camZC.CameraShake(forceOfShake, forceOfShake);
                    isF1InUse = true;
                }

                else if (timer < Time.time && isF1InUse == true)
                {

                    camZC.CameraShake(0, 0);
                    isMindManipulated = true;
                    
                }
                
            }

            else if (currentHit == null && isMindManipulated == false)
            {

                
                timer = Time.time + timerPossess;
                camZC.CameraShake(0, 0);
            }

        }

        else if (isFire1Triggered() == false)
        {
            isF1InUse = false;
            isContactControl = false;
            timerRipple = 0;
        }

        else if (Input.GetButtonDown("Fire1") == true || Input.GetAxis("Fire1") > 0 && isMindManipulated == true && isF1InUse == false)
        {

            isMindManipulated = false;
        }

        if (soFar == true) isMindManipulated = false;
            
    }

  

    void Fire2()
    {
        if (isMindManipulated == false)
        {
            if (isFire2Triggered() == true)
            {
               
                anim.SetTrigger("StartPossess");
                //anim.SetBool("isPossessing", true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Locked;
                normalCam.gameObject.SetActive(false);
                targetRotation.enabled = false;
                if ( currentHit == true && possParticles == null && soFar == false)
                {
                    possParticles = Instantiate(Resources.Load("ParticlePossession"), currentHit.transform.position, Quaternion.identity) as GameObject;
                    possParticles.transform.parent = currentHit.transform;
                }

                else if (currentHit == null  && possParticles == true) Destroy(possParticles);

            }


            else
            {

               
                //currentHit = null;
                if (possParticles == true) Destroy(possParticles);
                if (anim.GetCurrentAnimatorStateInfo(4).IsName("controlIDLE"))
                {
                    anim.SetTrigger("EndPossess");
                    anim.ResetTrigger("StartPossess");
                }

                //anim.SetBool("isPossessing", false);
                Cursor.visible = false;
                //Cursor.lockState = CursorLockMode.None;
                normalCam.gameObject.SetActive(true);
                targetRotation.enabled = true;


            }
        }

       


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

        else if (isMindManipulated == true && currentHit.transform != null)
        {
            possessionParticles.gameObject.SetActive(false);
        }


    }




}
