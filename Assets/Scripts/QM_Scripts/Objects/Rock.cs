using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour {

   


    //public GameObject player;
    //private float currentSpeedRot;
    //private float currentSpeed;
    //private float smoothWeight;
    //public float massPushed;
    //private float smoothSpeed;
    //public float speedSlowBB;
    //public float trueSpeedBB = 4;
    //Rigidbody rb;

    //EnnemiController enemyController;
    //FixedJoint joint;
    //bool isFixed;

    //private void Start()
    //{
    //    rb = GetComponent<Rigidbody>();

    //}

    //private void Update()
    //{


    //    //if (player.GetComponent<CharacterController>().DetectCollisions(.6f) == true)
    //    //{
    //    //    player.GetComponent<CharacterController>().speed = 0;
    //    //}

    //    //else if (player.GetComponent<CharacterController>().DetectCollisions(.6f) == false)
    //    //{
    //    //    player.GetComponent<CharacterController>().speed = 8;
    //    //}


    //    if (isFixed == true && Input.GetButtonUp("Fire3"))
    //    {
    //        rb.mass = 200000;
    //        isFixed = false;
    //    }
    //    Debug.DrawRay(transform.position, Vector3.up * -3);


    //}

    //private void FixedUpdate()
    //{

    //}

    //private void OnCollisionStay(Collision other)
    //{
    //    if (other.gameObject.tag == "Big Brainless" && MindPower.isMindManipulated == true)
    //    {
    //        if (Input.GetButton("Fire3"))
    //        {
    //            rb.MovePosition(transform.position + other.transform.forward * Time.deltaTime);
    //            transform.Rotate(other.transform.right * Time.deltaTime * 30);
    //        }

    //        if (Input.GetButtonDown("Fire3"))
    //        {
    //            isFixed = true;
    //            rb.mass = 1000;


    //        }




    //    }

    //}

    //private void OnCollisionEnter(Collision collision)
    //{


    //    if (collision.gameObject.layer == LayerMask.NameToLayer("Entity") && Crush() == true)
    //    {
    //        Debug.Log(rb.velocity.y);
    //        collision.transform.gameObject.SetActive(false);
    //    }
    //}



    //bool Crush()
    //{
    //    if (GetComponent<Rigidbody>().velocity.y < -6f)
    //    {

    //        return true;
    //    }

    //    else
    //        return false;
    //}

    //bool DetectFloot()
    //{
    //    RaycastHit hit;

    //    if (Physics.Raycast(transform.position, Vector3.up * -3, out hit ))
    //    {
    //        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
    //        {
    //            return true;
    //        }

    //    }
    //    return false;
    //}


}
