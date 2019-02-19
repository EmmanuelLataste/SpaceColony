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
    [SerializeField] public Transform followPlayer;
    [SerializeField] float radiusContactControl;
    [SerializeField] LayerMask ignoreCollider;
    static Transform hitControlled;
    Animator anim;
    bool isAlive;
    Collider[] contactControl;
    bool isContactControl;
    private void Start()
    {
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
    //    Gizmos.DrawRay(cameraMain.ScreenPointToRay(Input.mousePosition));
    //}

    void Update()
    {

        Control();
        ContactControl();
        if (currentHit != null && currentHit.gameObject.GetComponent<Life>().isAlive == false)
        {

           StartCoroutine(TransferWhenMMDied());

        }


    }

    IEnumerator TransferWhenMMDied()
    {
        Destroy(possParticles);
        yield return new WaitForSeconds(2);
        currentHit = null;

        anim.SetBool("isPossessing", false);
        camC.Follow(followPlayer);
        normalCam.enabled = true;
        cc.enabled = true;
        yield return null;
    }

    void Control()
    {
        if (anim.GetCurrentAnimatorStateInfo(2).IsName("Empty") == false && anim.GetCurrentAnimatorStateInfo(2).IsName("Throw") == false)
        {
            cc.canMove = false;
        }

        else cc.canMove = true;

        if (isFire1Triggered() == true)
        {
            if (isMindManipulated == true && canMindM == true)
            {
               
                Transfer(true, false, currentHit.GetComponent<PositionEnemies>().focusCamNormal.transform);
            }
               
            else if (isMindManipulated == false && canMindM == false)
            {

                Transfer(false, true, followPlayer);
            }
    

        }

        Fire1();
        Fire2();

    }

    

    public void ContactControlEvent()
    {
        isContactControl = true;
        anim.SetBool("isPossessing", false);
        if (isFire2Triggered() == false && contactControl.Length != 0)
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
            if (isContactControl == false) anim.SetBool("isPossessing", true);
      
        }
    }

   

    public bool isAiming2()
    {
        if (Physics.Raycast(cameraMain.ScreenPointToRay(Input.mousePosition), out hit, ignoreCollider))


        {

            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Entity") && hit.transform.gameObject.CompareTag("Player") == false)
            {
                if (isMindManipulated == false && isFire2Triggered() == true)

                {
                    if (isFire1Triggered() == true)
                    {
                        Debug.Log("aim");
                        camZC.CameraShake(forceOfShake, forceOfShake);
                    }

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

        else
        {

            camZC.CameraShake(0, 0);
        }
            
        return false;
    }

    public void  Transfer(bool enemyControlled, bool playerControlled, Transform followTransform)
    {

        anim.SetBool("isPossessing", enemyControlled);
        camC.Follow(followTransform);
        normalCam.enabled = true;
        cc.isControlled = playerControlled; 
        if (controledcc != null)
        {
            controledcc.isControlled = enemyControlled;
            controledcc.GetComponent<Rigidbody>().isKinematic = playerControlled;
            PositionEnemies pe = controledcc.GetComponent<PositionEnemies>();
            pe.transformPosition.GetComponent<NavMeshAgent>().enabled = playerControlled;
            pe.transformPosition.GetComponent<FieldOfView>().enabled = playerControlled;
            pe.transformPosition.GetComponent<EntityAI>().enabled = playerControlled;
            pe.transformPosition.GetComponent<Animator>().enabled = playerControlled;
            controledcc.GetComponent<Animator>().SetBool("AI", playerControlled);
            controledcc.GetComponent<Animator>().SetBool("Run", enemyControlled);
            //controledcc.GetComponent<PositionEnemies>().transformPosition.GetComponent<Animator>().SetBool("spot", playerControlled);
            //controledcc.GetComponent<PositionEnemies>().transformPosition.GetComponent<Animator>().SetBool("event", playerControlled);
            //controledcc.GetComponent<PositionEnemies>().transformPosition.GetComponent<Animator>().SetBool("isChasing", false);
            //controledcc.GetComponent<PositionEnemies>().transformPosition.GetComponent<Animator>().SetBool("isInvestigating", false);

            //controledcc.GetComponent<PositionEnemies>().transformPosition.GetComponent<FieldOfView>().target = null;
            //controledcc.GetComponent<PositionEnemies>().transformPosition.GetComponent<FieldOfView>().visibleTargets.Clear();
            
        }
       

        StartCoroutine(camC.CameraShakeTiming(forceOfShake / 2, forceOfShake / 2, .25f));
        canMindM = playerControlled;
        


    }


    float timer;
    void Fire1()
    {

        if (isFire1Triggered() == true && isMindManipulated == false)
        {
            

            if (isAiming2() == true && isFire2Triggered() == true)
            {
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

            else if (isAiming2() == false && isMindManipulated == false)
            {


                timer = Time.time + timerPossess;
                camZC.CameraShake(0, 0);
            }

        }

        else if (isFire1Triggered() == false)
        {
            isF1InUse = false;
            isContactControl = false;
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
