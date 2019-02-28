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
        
        //animator.gameObject.GetComponent<FieldOfView>().visible = false;

        //Without auto-barking the agent has continuous movment, the agent doesn't slow down when getting close to its destination point
        entityAgent.autoBraking = false;

        timer = Time.time + timerOffset;
        lastKnownPos = target.transform.position;
        //animator.GetComponent<EntityAI>().Investigate();
    }

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //entityAgent.destination = entity.transform.position;
        if (fov.visible == false )
        {
            if (timer > Time.time && fov.dstToTarget > 1)
            {
                animLinkedEntity.SetBool("Walk", true);
                entityAgent.destination = lastKnownPos;
               
               // entityAgent.transform.LookAt(lastKnownPos);

            }

            else if (timer <= Time.time || fov.dstToTarget <= 1)
            {
                animator.SetBool("targetVisible", false);
                animator.SetBool("targetFalse", false);
                target = null;
                fov.audibleTargets.Clear();
              
            }

        }

        else
        {
            if (fov.visibleTargets.Count != 0 )
            {
                animator.SetBool("isChasing", true);

            }

            else if (fov.audibleTargets.Count != 0 && fov.visibleTargets.Count == 0)
            {
                animator.SetBool("targetVisible", false);
            }

            else if (fov.target != null && fov.visibleTargets.Count == 0 && fov.audibleTargets.Count == 0)
            {
                
               
                if (timer < Time.time)
                {
                    animator.SetBool("targetVisible", false);
                    target = null;
                    
                }

                else
                {
                    animLinkedEntity.SetBool("Walk", true);
                    animLinkedEntity.SetBool("Run", false);
                    animLinkedEntity.SetBool("Attack", true);
                    //animator.transform.LookAt(fov.target.transform);
                    entityAgent.destination = target.transform.position;
                    entityAgent.isStopped = false;
                }
            }

            else
            {
                animator.SetBool("targetVisible", false);
            }

        }

        animator.SetBool("event", false);
    }

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetBool("isInvestigating", false);
        animator.SetBool("isChecking", false);
        animLinkedEntity.SetBool("Walk", false);

    }


    


}
