using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleTower : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Entity"))
        {
            other.GetComponent<CharacterController>().canBeManipulated = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Entity"))
        {
            other.GetComponent<CharacterController>().canBeManipulated = true;
        }
    }
}

   
