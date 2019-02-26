using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NeutralBehaviour : StateMachineBehaviour {


    GameObject entity;
    Transform[] waypoints;
    private int destPoint = 0;
    private int side = 0;

    public NavMeshAgent entityAgent;
    public bool isReversed;
    EntityAI entityAI;
    Animator animLinkedEntity;


    private void Awake()
    {
        //TEST OPTI
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animLinkedEntity = animator.GetComponent<EntityAI>().linkedEntity.GetComponent<Animator>();
        entity = animator.gameObject;
        isReversed = animator.gameObject.GetComponent<EntityAI>().isReversed;
        waypoints = animator.gameObject.GetComponent<EntityAI>().waypoints;
        entityAgent = animator.gameObject.GetComponent<NavMeshAgent>();
        animator.gameObject.GetComponent<FieldOfView>().target = null;
        animator.GetComponent<FieldOfView>().audible = false;
        entityAI = animator.gameObject.GetComponent<EntityAI>();
        animator.SetBool("event", false);
        animator.SetBool("spot", false);
        animator.SetBool("targetAudible", false);
        entityAgent.isStopped = true;

        animLinkedEntity.Rebind();
        animLinkedEntity.SetBool("Walk", false);


        animator.GetComponent<FieldOfView>().audibleTargets.Clear();
        animator.GetComponent<FieldOfView>().visible = false;
        animator.GetComponent<FieldOfView>().audible = false;


        //Without auto-barking the agent has continuous movment, the agent doesn't slow down when getting close to its destination point
        entityAgent.autoBraking = false;

        //ToNextWaypoint();
        if (waypoints.Length == 0)
        {

            entityAI.typePatrol = false;
        }

        else if (waypoints.Length != 0)
        {

            entityAI.typePatrol = true;
        }

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       

        if (animator.GetComponent<FieldOfView>().visible == true || animator.GetComponent<FieldOfView>().audible == true)
        {

            animator.SetBool("spot", true);
            animator.SetBool("event", true);
        }

        //if (animator.GetComponent<FieldOfView>().audible == true)
        //{
        //    animator.SetBool("targetAudible", true);
        //}
        //else
        //{
        //    animator.SetBool("targetAudible", false);
        //}

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animLinkedEntity.SetBool("Walk", false);
    }

    
}
