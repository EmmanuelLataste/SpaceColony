using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseBehaviour : StateMachineBehaviour {

    public List<Transform> visibleTargets = new List<Transform>();
    public NavMeshAgent entityAgent;
    
    private Vector3 lastKnownPos;
    FieldOfView fov;
    Animator animLinkedEntity;
    EntityAI entityAI;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animLinkedEntity = animator.GetComponent<EntityAI>().linkedEntity.GetComponent<Animator>();
        entityAI = animator.gameObject.GetComponent<EntityAI>();
        fov = animator.gameObject.GetComponent<FieldOfView>();

        visibleTargets = animator.gameObject.GetComponent<FieldOfView>().visibleTargets;
        entityAgent = animator.gameObject.GetComponent<NavMeshAgent>();
        if (visibleTargets.Count != 0)
        {
            entityAgent.destination = visibleTargets[0].transform.position;

            lastKnownPos = visibleTargets[0].transform.position;
        }



        animator.SetBool("isChasing", false);
        animator.SetBool("targetVisible", true);
        animLinkedEntity.Rebind();
        animLinkedEntity.SetBool("Run", true);
    }

	
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        

        if (Vector3.Distance(animator.transform.position, lastKnownPos) < 3 && animator.GetFloat("targetDst") > 5f)
        {
            Debug.Log("out of chase");
            animator.SetBool("targetVisible", false);
            entityAgent.speed = entityAI.beginAISpeed;
        }
        else if (visibleTargets.Count > 0 ) {

            entityAgent.destination = visibleTargets[0].transform.position;
            lastKnownPos = visibleTargets[0].transform.position;
            entityAgent.transform.LookAt(fov.target.transform);
            entityAgent.speed = fov.beginSpeed;
            


        }
        else if (visibleTargets.Count == 0 && animator.GetFloat("targetDst") >10) {
            entityAgent.destination = lastKnownPos;
            entityAgent.transform.LookAt(fov.target.transform);
            entityAgent.speed = entityAI.beginAISpeed;
            Debug.Log("liste vide");
        }

        if (visibleTargets.Count > 0 && fov.dstToTarget <= 2)
        {
            animLinkedEntity.SetBool("Attack", true);
        }

        else if (fov.dstToTarget > 2)
        {
            animLinkedEntity.SetBool("Attack", false);
        }

    }

	
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animLinkedEntity.SetBool("Run", false);
    }

	
}
