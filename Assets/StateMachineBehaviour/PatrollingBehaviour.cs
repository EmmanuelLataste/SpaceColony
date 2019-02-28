using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class PatrollingBehaviour : StateMachineBehaviour {

    GameObject entity;
    Transform[] waypoints;
    private int destPoint = 0;
    private int side = 0;

    public NavMeshAgent entityAgent;
    public bool isReversed;
    EntityAI entityAI;
    Animator animLinkedEntity;


    private void Awake() {
        //TEST OPTI
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
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
        
        animLinkedEntity.Rebind();
        animLinkedEntity.SetBool("Walk", true);

        animator.GetComponent<FieldOfView>().audibleTargets.Clear();

        animator.GetComponent<FieldOfView>().visible = false;
        animator.GetComponent<FieldOfView>().audible = false;

        //Without auto-barking the agent has continuous movment, the agent doesn't slow down when getting close to its destination point
        entityAgent.autoBraking = false;
        
        ToNextWaypoint();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animLinkedEntity.SetBool("Run", false);
        if (entityAI.isMoving == true)
        {
            entityAgent.isStopped = false;

            animLinkedEntity.SetBool("Walk", true);
            if (animator.GetComponent<FieldOfView>().visible == true || animator.GetComponent<FieldOfView>().audible == true)
            {

                animator.SetBool("spot", true);
                animator.SetBool("event", true);
            }

            if (animator.GetComponent<FieldOfView>().audible == true)
            {
                animator.SetBool("targetAudible", true);
            }
            else
            {
                animator.SetBool("targetAudible", false);
            }

            if (!entityAgent.pathPending && entityAgent.remainingDistance < 0.5f)
            {
                ToNextWaypoint();

            }


            entityAgent.speed = entityAI.beginAISpeed;
        }
    
        
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animLinkedEntity.SetBool("Walk", false);
    }

    public void ToNextWaypoint() {
        //In case of no attribuated waypoint : return

        if (waypoints.Length == 0) {
            return;
        }

        //Say to the agent to go to this currently selected waypoint
        entityAgent.destination = waypoints[destPoint].transform.position;

        //Define the destination point of the agent
        //Cycling to start if necessary
        if (isReversed == false) {
            destPoint = (destPoint + 1) % waypoints.Length;
        }

        if (isReversed == true) {
            if (destPoint == waypoints.Length - 1) {
                side = 0;
            }

            if (destPoint == 0) {
                side = 1;
            }

            if (side == 0) {
                destPoint = destPoint - 1;
            }

            if (side == 1) {
                destPoint = destPoint + 1;
            }

        }

    }


}
