using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{

    public float healthPoints;
    public bool isAlive = true;
    Rigidbody rb;
    bool isGoingToDie = false;
    bool onceDead;
    Animator anim;
    CharacterController cc;
    PositionEnemies pe;
    [SerializeField] GameObject player;
    [SerializeField] GameObject deathPosition;



    private void Start()
    {
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

        if (healthPoints <= 0)
        {
          
            isAlive = false;

        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
       
        if (isGoingToDie == true)
        {
            isAlive = false;
        }
       
    }


    void Dead()
    {


        if (isAlive == false && onceDead == false)
        {
            

            if (MindPower.currentHit == null && gameObject.tag == "Player")
                CameraController.cam.GetComponent<CameraController>().Follow(deathPosition.transform);
            else if (MindPower.currentHit != null && gameObject.tag != "Player")
                CameraController.cam.GetComponent<CameraController>().Follow(MindPower.currentHit.GetComponent<Life>().deathPosition.transform);
            else if (MindPower.currentHit != null && gameObject.tag == "Player")
            {
                
                CameraController.cam.GetComponent<CameraController>().Follow(deathPosition.transform);
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
            
        }
        if (rb.velocity.y <= -25)
        {

           // isGoingToDie = true;
        }

    }
}

