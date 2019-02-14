using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {

    Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>() == true && other.GetComponent<CharacterController>().otherGameObject.tag == "KeyCard")
        {
            animator.SetTrigger("Open");

            Debug.Log("Door has been unlocked");
        }
    }
}
