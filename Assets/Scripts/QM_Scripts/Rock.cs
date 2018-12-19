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
    private float smoothSpeed;
    public float speedSlowBB;
    private float trueSpeed = 4;
    private MindPower mindPower;

    private void Start()
    {
        mindPower = player.GetComponent<MindPower>();
    }

    private void Update()
    {

        if (player.GetComponent<CharacterController>().DetectCollisions(.6f) == true)
        {
            player.GetComponent<CharacterController>().speed = 0;
        }

        else if (player.GetComponent<CharacterController>().DetectCollisions(.6f) == false)
        {
            player.GetComponent<CharacterController>().speed = 8;
            //player.GetComponent<CharacterController>().speed = Mathf.Lerp(0, 8, smoothSpeed);
            //smoothSpeed += player.GetComponent<CharacterController>().smoothSpeedPlayerMove * Time.deltaTime;
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Big Brainless" && mindPower.isMindManipulated == true)
        {
            GetComponent<Rigidbody>().mass = massPushed;

            other.GetComponent<EnnemiController>().speed = speedSlowBB;

        }

       

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Big Brainless" && mindPower.isMindManipulated == true)
        {
            other.GetComponent<EnnemiController>().speed = trueSpeed;
            GetComponent<Rigidbody>().mass = 1000000000;
        }

        
    }

    private void OnCollisionEnter(Collision collision)
    {
   

        if (collision.gameObject.layer == LayerMask.NameToLayer("Entity") && Crush() == true)
        {
            player.GetComponent<MindPower>().isMindManipulated = false;
            Destroy(collision.gameObject);
        }
    }

   

    bool Crush()
    {
        if (GetComponent<Rigidbody>().velocity.y < -.3f)
        {

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
