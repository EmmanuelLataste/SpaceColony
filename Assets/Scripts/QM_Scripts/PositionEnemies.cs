using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PositionEnemies : MonoBehaviour {


    public GameObject transformPosition;
    public GameObject focusCamNormal;
    Animator anim;
    [SerializeField]  Animator nmAnim;
    NavMeshAgent nma;
    CharacterController cc;

	// Use this for initialization
	void Start ()

    {
        cc = GetComponent<CharacterController>();
        nma = transformPosition.GetComponent<NavMeshAgent>();
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

        AIAnimation();
	}

    void AIAnimation()
    {
        

        if (nmAnim.GetCurrentAnimatorStateInfo(0).IsName("Patrolling"))
        {
            anim.SetBool("Walk", true);
            nma.speed = cc.beginSpeed / 4;
            
        }

        else anim.SetBool("Walk", false);


        if (nmAnim.GetCurrentAnimatorStateInfo(0).IsName("Chase"))
        {
            anim.SetBool("Run", true);
            nma.speed = cc.beginSpeed;
        }


        if (nmAnim.GetCurrentAnimatorStateInfo(0).IsName("Investigating"))
        {
            anim.SetTrigger("Spot");
        }


    }
}
