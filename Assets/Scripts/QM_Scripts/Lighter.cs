using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighter : Ignitable, IDrop, IPickUp {
    private GameObject player;
    public bool isHolding = false;

    private void Update()
    {
        Drop();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerStay( Collider other)
    {
        if (other.gameObject == player && isHolding == false && FindObjectOfType<CharacterController>().isHolding == false)
        {
            PickUp(player);

        }

        
    }

    public void PickUp(GameObject player)
    {
        if (Input.GetButtonUp("X"))
        {
            transform.rotation = player.transform.rotation;
            transform.position = player.transform.position + transform.right;
            transform.parent = player.transform;

            GetComponent<Rigidbody>().detectCollisions = false;
            GetComponent<Rigidbody>().isKinematic = true;

            FindObjectOfType<CharacterController>().isHolding = true;
            isHolding = true;
        }
    }

    public void Drop()
    {
        if (Input.GetButtonDown("B") && isHolding == true)
        {
            Debug.Log("Drop");
            transform.parent = null;

            GetComponent<Rigidbody>().detectCollisions = true;
            GetComponent<Rigidbody>().isKinematic = false;

            FindObjectOfType<CharacterController>().isHolding = false;
            isHolding = false;
        }
    }


}
