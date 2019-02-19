using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SuspiciousBehavior : StateMachineBehaviour {
    public List<Transform> visibleTargets = new List<Transform>();

    Animator animLinkedEntity;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animLinkedEntity = animator.GetComponent<EntityAI>().linkedEntity.GetComponent<Animator>();
        animLinkedEntity.Rebind();

        //animator.gameObject.GetComponent<FieldOfView>().visible = false;

        //animator.SetBool("targetVisible", false);
        //animator.SetBool("spot", false);

        //if (animator.GetComponent<FieldOfView>().audible == true) {
        //    animator.GetComponent<EntityAI>().Suspect();
        //}
        //else if (animator.GetComponent<FieldOfView>().visible == true) {
        //    animator.SetBool("targetVisible", true);
        //} 
        //else if (animator.GetComponent<FieldOfView>().visible == false) {
        //    animator.SetBool("targetVisible", false);
        //}  
        if (animator.GetBool("event") == true)
        {
             animator.gameObject.GetComponent<EntityAI>().Suspicious();
            Debug.Log("heello");
        }

        else animator.gameObject.GetComponent<EntityAI>().Suspicious2();


        visibleTargets = animator.gameObject.GetComponent<FieldOfView>().visibleTargets;
   

    }

	
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
        if (visibleTargets.Count != 0)
        {
            animator.transform.LookAt(visibleTargets[0].gameObject.transform);
            animator.gameObject.GetComponent<FieldOfView>().target = visibleTargets[0].gameObject;
        }
       
    }

	
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.gameObject.GetComponent<NavMeshAgent>().isStopped = false;
    }


}
