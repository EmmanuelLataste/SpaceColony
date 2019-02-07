using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseBehaviour : StateMachineBehaviour {

    public List<Transform> visibleTargets = new List<Transform>();
    public NavMeshAgent entityAgent;

    private Vector3 lastKnownPos;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        visibleTargets = animator.gameObject.GetComponent<FieldOfView>().visibleTargets;
        entityAgent = animator.gameObject.GetComponent<NavMeshAgent>();

        entityAgent.destination = visibleTargets[0].transform.position;
        lastKnownPos = visibleTargets[0].transform.position;

        animator.SetBool("isChasing", false);
        animator.SetBool("targetVisible", true);
    }

	
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        

        if (Vector3.Distance(animator.transform.position, lastKnownPos) < 3)
        {
            Debug.Log("out of chase");
            animator.SetBool("targetVisible", false);
        }
        else if (visibleTargets.Count > 0) {
            entityAgent.destination = visibleTargets[0].transform.position;
            lastKnownPos = visibleTargets[0].transform.position;
        }
        else if (visibleTargets.Count == 0) {
            entityAgent.destination = lastKnownPos;            
            Debug.Log("liste vide");
        }

    }

	
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	
	}

	
}
