using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{

    public float healthPoints;
    float basicHP;
    float startingHP;
    public bool isAlive = true;
    Rigidbody rb;
    bool isGoingToDie = false;
    bool onceDead;
    Animator anim;
    CharacterController cc;
    PositionEnemies pe;
    [SerializeField] GameObject player;
    [SerializeField] GameObject deathPosition;
    [SerializeField] float invulnerabilityTime;
    bool isAttacked;
    [SerializeField] float timerLifeRecovery;
    float TLFOffset;
    [SerializeField] LayerMask fallCollision;



    private void Start()
    {
        
        startingHP = healthPoints;
        basicHP = healthPoints;
        cc = GetComponent<CharacterController>();
        if (GetComponent<PositionEnemies>())
        {
            pe = GetComponent<PositionEnemies>();
        }

        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        Dead();
        HealthRecovery();
        //Fall();


        if (healthPoints <= 0)
        {
          
            isAlive = false;
            if (gameObject.tag == "Player")
            {

                CameraController.cam.GetComponent<CameraController>().Follow(deathPosition.transform);
            }

        }

        if (isAttacked == true)
        {
            StartCoroutine(InvulnerabiltyFrames());
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
       
        if (isGoingToDie == true/* && collision.gameObject.layer == fallCollision*/)
        {
            isAlive = false;
        }
       
    }

    IEnumerator InvulnerabiltyFrames()
    {
        if (basicHP != healthPoints )
        {
            basicHP = healthPoints;
            yield return new WaitForSeconds(invulnerabilityTime);
            isAttacked = false;

        }
        yield return null;
    }

    public void Damages(float damages)
    {
        if (isAttacked == false)
        {
            healthPoints -= damages;
            anim.SetTrigger("Hitted");
            TLFOffset = Time.time + timerLifeRecovery;
            isAttacked = true;
            

        }

    }

    void HealthRecovery()
    {
        if (startingHP != healthPoints)
        {
            if (TLFOffset < Time.time)
            {
                TLFOffset = Time.time + timerLifeRecovery;
                healthPoints = startingHP;

            }
            
        }
    }

    void Fall()
    {
        if (rb.velocity.y <= -25 && isGoingToDie == false)
        {

            isGoingToDie = true;
        }

        else if (rb.velocity.y > -25 && isGoingToDie == true)
        {
            isGoingToDie = false;
        }

    }


    void Dead()
    {


        if (isAlive == false && onceDead == false)
        {

            if (MindPower.currentHit == null && gameObject.tag == "Player")
                CameraController.cam.GetComponent<CameraController>().Follow(deathPosition.transform);
            else if (MindPower.currentHit != null && gameObject.tag != "Player")
            {
                CameraController.cam.GetComponent<CameraController>().Follow(MindPower.currentHit.GetComponent<Life>().deathPosition.transform);
                if (GetComponent<AudioListener>() == true) Destroy(GetComponent<AudioListener>());
            }
               
            
               



            if (GetComponent<PositionEnemies>())
            {
                Destroy(pe.transformPosition);
                Destroy(pe);
            }

            anim.enabled = false;

            Destroy(cc);
            gameObject.layer = LayerMask.NameToLayer("RagDoll");
            onceDead = true;
            rb.velocity = Vector3.zero ;
            
        }
       
    }
}

