using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : Flammable, IPickUp, IDrop
{
    private GameObject player;
    public bool isHolding = false;
    public float woods;
    private bool isBurning = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        Drop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Ignitable>() == true && isBurning == false)
        {
            Burn();
            isBurning = true;
        }

        if (other.gameObject.tag == "Wood")
        {
            woods++;
            woods = woods / woods;
        }

        if (other.gameObject.GetComponent<Insect>() == true && isBurning == true)
        {
            Destroy(other.gameObject);
            Debug.Log("BOOOOOM");
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Wood")
        {
            woods--;
            if ( woods > 0)
            {
                woods = woods / woods;
            }
           
        }
    }

    private void OnTriggerStay(Collider other)
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
            player.transform.Rotate(new Vector3(90, 0, 0));
            transform.position = player.transform.position + transform.right;
            transform.parent = player.transform;

            GetComponent<Rigidbody>().detectCollisions = false;
            GetComponent<Rigidbody>().isKinematic = true;

            FindObjectOfType<CharacterController>().isHolding = true;
            isHolding = true;
            Debug.Log("Hold");
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


