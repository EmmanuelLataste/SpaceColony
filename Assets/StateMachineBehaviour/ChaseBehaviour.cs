using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseBehaviour : StateMachineBehaviour {

    public List<Transform> visibleTargets = new List<Transform>();
    public NavMeshAgent entityAgent;

    private Transform lastKnownPos;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        visibleTargets = animator.gameObject.GetComponent<FieldOfView>().visibleTargets;
        entityAgent = animator.gameObject.GetComponent<NavMeshAgent>();

        entityAgent.destination = visibleTargets[0].transform.position;
        lastKnownPos = visibleTargets[0].transform;
    }

	
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Debug.Log(visibleTargets.Count);
        Debug.Log(lastKnownPos);

        if (visibleTargets.Count > 0) {
            entityAgent.destination = visibleTargets[0].transform.position;
            lastKnownPos = visibleTargets[0].transform;
        }
        else if (visibleTargets.Count == 0) {
            entityAgent.destination = lastKnownPos.transform.position;
            Debug.Log("liste vide");
        }

    }

	
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	
	}

	
}
