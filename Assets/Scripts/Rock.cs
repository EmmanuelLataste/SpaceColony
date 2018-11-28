using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour {
    private FixedJoint joint;
    private bool isJoin;
    public GameObject player;
    private float currentSpeedRot;
    private float smoothWeight;
    public float massPushed;

    private void Update()
    {
        Debug.Log(GetComponent<Rigidbody>().velocity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Big Brainless")
        {
            GetComponent<Rigidbody>().mass = massPushed;
        }

        

        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Big Brainless")
        {
            GetComponent<Rigidbody>().mass = 200000;
        }

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ( collision.gameObject.tag == "Player")
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }

        if (collision.gameObject.layer == 11 && Crush() == true)
        {
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    bool Crush()
    {
        if (GetComponent<Rigidbody>().velocity.y < -.1f)
        {
            Debug.Log(GetComponent<Rigidbody>().velocity.y);
            return true;
        }

        else
            return false;
    }

    //private void Start()
    //{
    //    currentSpeedRot = player.GetComponent<CharacterController>().speedRotationPlayer;
    //}

    //private void Update()
    //{

    //    //if (Input.GetButtonUp("X") && joint != null)
    //    //{
    //    //    //Destroy(joint);
    //    //    //player.GetComponent<CharacterController>().speedRotationPlayer = currentSpeedRot;
    //    //    //GetComponent<Rigidbody>().mass = 100000f;
    //    //}

    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.layer == 11 && Input.GetButton("X")) 
    //    {
    //        isJoin = true;
    //        //other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;

    //        //gameObject.AddComponent<FixedJoint>();
    //        //joint = GetComponent<FixedJoint>();
    //        //joint.connectedBody = player.GetComponent<Rigidbody>();

    //        player.GetComponent<CharacterController>().speedRotationPlayer = 0.001f;
    //        //player.GetComponent<CharacterController>().smoothRotationPlayer = 0;




    //    }



    //}

    //private void OnTriggerExit(Collider other)
    //{
    //        if (other.gameObject.layer == 11)
    //    {
    //        isJoin = false;
    //        player.GetComponent<CharacterController>().speedRotationPlayer = currentSpeedRot;
    //        //other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
    //    }



    //}




}
