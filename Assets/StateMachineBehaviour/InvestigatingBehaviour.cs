using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InvestigatingBehaviour : StateMachineBehaviour {

    public GameObject entity;
    public List<Transform> visibleTargets = new List<Transform>();
    public NavMeshAgent entityAgent;
    private Vector3 lastKnownPos;
    FieldOfView fov;
    GameObject target;
     float timer;
    [SerializeField]  float timerOffset;
    Animator animLinkedEntity;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animLinkedEntity = animator.GetComponent<EntityAI>().linkedEntity.GetComponent<Animator>();
        target = animator.gameObject.GetComponent<FieldOfView>().target;
        fov = animator.gameObject.GetComponent<FieldOfView>();
        entity = animator.gameObject;
        entityAgent = animator.gameObject.GetComponent<NavMeshAgent>();
        animator.gameObject.GetComponent<FieldOfView>().visibleTargets.Clear();
        visibleTargets = animator.gameObject.GetComponent<FieldOfView>().visibleTargets;

        animator.SetBool("spot", false);
        animator.SetBool("isChasing", false);
        animator.SetBool("targetVisible", true);
        animator.SetBool("targetAudible", false);

        animLinkedEntity.Rebind();
        animLinkedEntity.SetBool("Walk", true);
        //animator.gameObject.GetComponent<FieldOfView>().visible = false;

        //Without auto-barking the agent has continuous movment, the agent doesn't slow down when getting close to its destination point
        entityAgent.autoBraking = false;

        timer = Time.time + timerOffset;
        lastKnownPos = target.transform.position;
        //animator.GetComponent<EntityAI>().Investigate();
    }

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //entityAgent.destination = entity.transform.position;
        if (fov.visible == false)
        {
            if (timer > Time.time)
            {
                entityAgent.destination = lastKnownPos;
               
               // entityAgent.transform.LookAt(lastKnownPos);

            }

            else
            {
                animator.SetBool("targetVisible", false);
               
            }

        }

        else
        {
            animator.SetBool("isChasing", true);
          
        }

        animator.SetBool("event", false);
    }

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetBool("isInvestigating", false);
        animator.SetBool("isChecking", false);
        animLinkedEntity.SetBool("Walk", false);

    }


    


}
