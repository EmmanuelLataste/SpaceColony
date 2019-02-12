﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InvestigatingBehaviour : StateMachineBehaviour {

    public GameObject entity;
    public List<Transform> visibleTargets = new List<Transform>();
    public NavMeshAgent entityAgent;

        
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        entity = animator.gameObject;
        entityAgent = animator.gameObject.GetComponent<NavMeshAgent>();
        visibleTargets = animator.gameObject.GetComponent<FieldOfView>().visibleTargets;

        animator.SetBool("spot", false);
        animator.SetBool("isChasing", false);
        animator.SetBool("targetVisible", true);
        animator.SetBool("targetAudible", false);

        //Without auto-barking the agent has continuous movment, the agent doesn't slow down when getting close to its destination point
        entityAgent.autoBraking = false;
        animator.GetComponent<EntityAI>().Investigate();
    }

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
         entityAgent.destination = entity.transform.position;                
    }

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	
	}


    


}
