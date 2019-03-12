using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SuspiciousBehavior : StateMachineBehaviour {
    public List<Transform> visibleTargets = new List<Transform>();
    public List<Transform> audibleTargets = new List<Transform>();
    public NavMeshAgent entityAgent;
    Animator animLinkedEntity;
    FieldOfView fov;
    Material crystalMat;
    Color suspiciousColor;
    float timerLerp;
    [SerializeField] float timerLerpOffset = 1;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animLinkedEntity = animator.GetComponent<EntityAI>().linkedEntity.GetComponent<Animator>();
        animLinkedEntity.Rebind();
        entityAgent = animator.gameObject.GetComponent<NavMeshAgent>();
        fov = animator.gameObject.GetComponent<FieldOfView>();
        crystalMat = animator.gameObject.GetComponent<EntityAI>().crystalMat;
        suspiciousColor = animator.gameObject.GetComponent<EntityAI>().suspiciousColor;

        if (animator.GetBool("event") == true && fov.dstToTarget > 3)
        {
            animator.gameObject.GetComponent<EntityAI>().Suspicious();
            Debug.Log("suspi");
        }

        else if (animator.GetBool("event") == false)
        {

            animator.gameObject.GetComponent<EntityAI>().Suspicious2();
        } 

        else if (animator.GetBool("event") == true && fov.dstToTarget <= 3)
        {
            Debug.Log("invest");
            animator.SetBool("isInvestigating", true);
        }


        visibleTargets = animator.gameObject.GetComponent<FieldOfView>().visibleTargets;
        audibleTargets = animator.gameObject.GetComponent<FieldOfView>().audibleTargets;
        animLinkedEntity.gameObject.GetComponent<CharacterController>().canChangeColor = false;

    }

	
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        crystalMat.SetVector("_EmissionColor", Vector4.Lerp(Color.white * 2.25f, suspiciousColor * 1.75f, timerLerp));
        timerLerp += timerLerpOffset * Time.deltaTime;
        animator.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
        if (visibleTargets.Count != 0 && animator.GetBool("event") == true )
        {
            //animator.transform.LookAt(visibleTargets[0].gameObject.transform);
            animator.gameObject.GetComponent<FieldOfView>().target = visibleTargets[0].gameObject;

        }

        else if (audibleTargets.Count != 0 && visibleTargets.Count == 0 && animator.GetBool("event") == true)
        {
            //animator.transform.LookAt(audibleTargets[0].gameObject.transform);
            animator.gameObject.GetComponent<FieldOfView>().target = audibleTargets[0].gameObject;
        }

        else if (fov.target == true && animator.GetBool("event") == true)
        {
            //animator.transform.LookAt(fov.target.gameObject.transform);
        }
       
    }

	
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.gameObject.GetComponent<NavMeshAgent>().isStopped = false;
    }


}
