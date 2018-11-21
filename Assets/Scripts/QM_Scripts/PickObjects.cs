using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickObjects : MonoBehaviour {
    private bool isHolding = false;
    private bool canCarry = false;
    private GameObject currentOther;
    private string other2;
    private void Update()
    {
        Drop();
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetButtonDown("X") && isHolding == false)

        {

            if (other.gameObject.tag == "Wood")
               
            {
                
                Hold(transform, new Vector3(90, transform.rotation.y, transform.rotation.z), new Vector3(1, .1f, 0), other.gameObject);
                

            }

            else if (other.gameObject.tag == "Lighter")
            {
                Hold(transform, new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z), new Vector3(1, .1f, 0), other.gameObject);
            }

            
        }
        
    }

    void Drop()
    {
        if (Input.GetButtonDown("B") && isHolding == true)
        {
            Debug.Log("Hello");
            currentOther.transform.parent = null;
            currentOther.gameObject.GetComponent<Rigidbody>().detectCollisions = true;
            currentOther.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            isHolding = false;
        }
    }

    void Hold(Transform transform, Vector3 rotation, Vector3 position, GameObject other)
    {
        other.transform.parent = transform;
        other.transform.position = transform.position + position;
        other.transform.rotation = Quaternion.Euler(rotation);
        other.gameObject.GetComponent<Rigidbody>().detectCollisions = false;
        other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        currentOther = other.gameObject;
        isHolding = true;
    }






}
