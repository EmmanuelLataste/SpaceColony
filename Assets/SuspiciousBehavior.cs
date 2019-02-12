using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspiciousBehavior : StateMachineBehaviour {


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (animator.GetComponent<FieldOfView>().visible == true) {
            animator.SetBool("targetVisible", true);
        } else if (animator.GetComponent<FieldOfView>().visible == false) {
            animator.SetBool("targetVisible", false);
        }       
    }

	
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        
    }

	
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	
	}


}
