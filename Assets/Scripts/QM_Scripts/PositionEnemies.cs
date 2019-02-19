using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PositionEnemies : MonoBehaviour {


    public GameObject transformPosition;
    public GameObject focusCamNormal;
    Animator anim;
    Animator nmAnim;
    NavMeshAgent nma;
    CharacterController cc;
    Rigidbody rb;

	// Use this for initialization
	void Start ()

    {
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CharacterController>();
        nma = transformPosition.GetComponent<NavMeshAgent>();
        nmAnim = transformPosition.GetComponent<Animator>();
        anim = GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {
        if (transformPosition != null && MindPower.isMindManipulated == false)
        {
            transform.position = transformPosition.transform.position;
            transform.rotation = transformPosition.transform.rotation;
        }

        else if (transformPosition != null && MindPower.isMindManipulated == true)
        {
            
            transformPosition.transform.position = transform.position;
            transformPosition.transform.rotation = transform.rotation;
        }

        else
        {
            Destroy(gameObject);
            Destroy(gameObject.transform.parent.gameObject);
        }
        if (cc.isControlled == false)
        {
            //AIAnimation();
        }

	}

    void AIAnimation()
    {
        if (cc.isControlled == false)
        {
            if (nmAnim.GetCurrentAnimatorStateInfo(0).IsName("Patrolling"))
            {
                anim.SetBool("Walk", true);
                anim.SetBool("Run", false);
                nma.speed = cc.beginSpeed / 6;

            }

            else anim.SetBool("Walk", false);


            if (nmAnim.GetCurrentAnimatorStateInfo(0).IsName("Chase"))
            {
                anim.SetBool("Run", true);
                anim.SetBool("Walk", false);
                nma.speed = cc.beginSpeed;

            }


            if (nmAnim.GetCurrentAnimatorStateInfo(0).IsName("Investigating"))
            {
                anim.SetBool("Walk", true);
                anim.SetBool("Run", false);
                nma.speed = cc.beginSpeed / 4;
            }

            if (nmAnim.GetFloat("targetDst") <= 2 && nmAnim.GetBool("isChasing") == true)
            {
                anim.SetBool("Attack", true);

            }

            else if (nmAnim.GetFloat("targetDst") > 2.01f && nmAnim.GetBool("isChasing") == true)
            {
                anim.SetBool("Attack", false);

            }

            if (nmAnim.GetCurrentAnimatorStateInfo(0).IsName("Suspicious"))
            {
                anim.SetBool("Walk", false);
                anim.SetBool("Run", false);
            }
            

        }




    }
}
