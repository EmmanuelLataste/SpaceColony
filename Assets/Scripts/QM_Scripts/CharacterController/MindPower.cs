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

    public float timerPossess = 2;
    [Range(0, 5)]
    public float forceOfShake;
    float currentHitSpeed;


    [Header("Camera")]
    public Camera cameraMain;
    public CinemachineVirtualCamera normalCam;
    public CinemachineVirtualCamera zoomCam;
    private bool isZoom = false;
    bool canMindM = true;
    bool isPossessionParticles;
    GameObject possParticles;
    bool isF1InUse;
    CharacterController cc;
    CharacterController controledcc;
    CameraZoomController camZC;
    CameraController camC;
    [SerializeField] Transform followPlayer;
    [SerializeField] float radiusContactControl;
    static Transform hitControlled;
    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
        camC = normalCam.GetComponent<CameraController>();
        camZC = zoomCam.GetComponent<CameraZoomController>();
        cc = GetComponent<CharacterController>();

        currentHit = null;
        hitControlled = transform;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawRay(cameraMain.ScreenPointToRay(Input.mousePosition));
    }

    void Update()
    {

        Control();
        ContactControl();
        if (currentHit != null && currentHit.gameObject.activeSelf == false)
        {
            currentHit = null;
            Debug.Log("DEADEA");
            Transfer(false, true, transform.Find("FocusCamZoom").transform);

        }
    }
    
    void Control()
    {
        if (isFire1Triggered() == true)
        {
            if (isMindManipulated == true && canMindM == true)
                Transfer(true, false, currentHit.Find("FocusCamZoom").transform);
            else if (isMindManipulated == false && canMindM == false)
                Transfer(false, true, followPlayer);

        }

        Fire1();
        Fire2();

    }

  

    void ContactControl()
    {
        if (isFire1Triggered() == true && isFire2Triggered() == false && isMindManipulated == false)
        {
            Collider[] contactControl = Physics.OverlapSphere(transform.position, radiusContactControl);
            foreach(Collider contactCol in contactControl)
            {
                if (contactCol.gameObject.layer == LayerMask.NameToLayer ("Entity") && contactCol.gameObject.CompareTag("Player") == false && isF1InUse == false)
                {
                    isF1InUse = true;
                    currentHit = contactCol.transform;
                    isMindManipulated = true;
                    controledcc = contactCol.GetComponent<CharacterController>();

                }
            }
        }
    }

   

    public bool isAiming2()
    {
        if (Physics.Raycast(cameraMain.ScreenPointToRay(Input.mousePosition), out hit))


        {

            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Entity") && hit.transform.gameObject.CompareTag("Player") == false)
            {
                if (isMindManipulated == false && isFire2Triggered() == true)

                {
                    if (isFire1Triggered() == true) camZC.CameraShake(forceOfShake, forceOfShake);
                    currentHit = hit.transform;
                    controledcc = currentHit.GetComponent<CharacterController>();

                }

                return true;
            }
            else
            {

                currentHit = null;
                camZC.CameraShake(0, 0);
               

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

        else camZC.CameraShake(0, 0);
        return false;
    }

    void Transfer(bool enemyControlled, bool playerControlled, Transform followTransform)
    {
        anim.SetBool("isPossessing", enemyControlled);
        camC.Follow(followTransform);
        normalCam.enabled = true;
        cc.enabled = playerControlled; 
        if (controledcc != null)
        {
            controledcc.enabled = enemyControlled;
            controledcc.GetComponent<Rigidbody>().isKinematic = playerControlled;
            PositionEnemies pe = controledcc.GetComponent<PositionEnemies>();
            pe.transformPosition.GetComponent<NavMeshAgent>().enabled = playerControlled;
            pe.transformPosition.GetComponent<FieldOfView>().enabled = playerControlled;
            pe.transformPosition.GetComponent<EntityAI>().enabled = playerControlled;
            pe.transformPosition.GetComponent<Animator>().enabled = playerControlled;
        }
       
        //controledcc.GetComponent<NavMeshAgent>().enabled = playerControlled;
        //controledcc.GetComponent<FieldOfView>().enabled = playerControlled;
        //controledcc.GetComponent<EntityAI>().enabled = playerControlled;
        //controledcc.GetComponent<Animator>().enabled = playerControlled;
        StartCoroutine(camC.CameraShakeTiming(forceOfShake / 2, forceOfShake / 2, .25f));
        canMindM = playerControlled;


    }
    //IEnumerator MindManipulationTime()
    //{
    //    StartCoroutine(camZC.CameraShakeTiming(forceOfShake, forceOfShake, timerPossess));
    //    yield return new WaitForSeconds(timerPossess);
    //    isMindManipulated = true;
    //    yield return null;
    //}

    float timer;
    void Fire1()
    {

        if (isFire1Triggered() == true && isMindManipulated == false)
        {
            if (isAiming2() == true && isFire2Triggered() == true)
            {
                if (timer < Time.time && isF1InUse == false)
                {
                    Debug.Log("1");
                    timer = Time.time + timerPossess;
                    camZC.CameraShake(forceOfShake, forceOfShake);
                    isF1InUse = true;
                }

                else if (timer < Time.time && isF1InUse == true)
                {
                    Debug.Log("2");
                    camZC.CameraShake(0, 0);
                    isMindManipulated = true;
                    
                }
                
            }

            else if (isAiming2() == false)
            {

                Debug.Log("3");
                timer = Time.time + timerPossess;
                camZC.CameraShake(0, 0);
            }

        }

        else if (isFire1Triggered() == false)
        {
            isF1InUse = false;
        }

        else if (Input.GetButtonDown("Fire1") == true || Input.GetAxis("Fire1") > 0 && isMindManipulated == true && isF1InUse == false)
        {

            isMindManipulated = false;
        }
            
    }

    void Fire2()
    {
        if (isMindManipulated == false)
        {
            if (isFire2Triggered() == true)
            {
                anim.SetBool("isPossessing", true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Locked;
                normalCam.enabled = false;
                if (isAiming2() == true && possParticles == null)
                {
                    possParticles = Instantiate(Resources.Load("ParticlePossession"), currentHit.transform.position, Quaternion.identity) as GameObject;
                    possParticles.transform.parent = currentHit.transform;
                }

                else if (isAiming2() == false  && possParticles == true) Destroy(possParticles);

            }


            else
            {
                //currentHit = null;
                if (possParticles == true) Destroy(possParticles);
                anim.SetBool("isPossessing", false);
                Cursor.visible = false;
                //Cursor.lockState = CursorLockMode.None;
                normalCam.enabled = true;


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
