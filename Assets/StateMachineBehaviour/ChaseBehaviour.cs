﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseBehaviour : StateMachineBehaviour {

    public List<Transform> visibleTargets = new List<Transform>();
    public NavMeshAgent entityAgent;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        visibleTargets = animator.gameObject.GetComponent<FieldOfView>().visibleTargets;
        entityAgent = animator.gameObject.GetComponent<NavMeshAgent>();       
	}

	
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        entityAgent.destination = visibleTargets[0].transform.position;
    }

	
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	
	}

	
}
