using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBrainless : MonoBehaviour {

    // Use this for initialization

    RaycastHit hitDetection;
    [SerializeField] float lengthDetection;
    [SerializeField] float speedPush;
    [SerializeField] LayerMask maskDetection;
    float beginSpeed;
    float speed;
    bool detect;
    Rigidbody rb;


    private void FixedUpdate()
    {
        rb = GetComponent<Rigidbody>();
        beginSpeed = GetComponent<CharacterController>().beginSpeed;
        speed = GetComponent<CharacterController>().speed;
        Push();
    }

    public bool DetectCollisions()
    {

        if (Physics.Raycast(transform.position + transform.up, transform.forward, out hitDetection, lengthDetection))
        {
            if (hitDetection.transform.gameObject.tag == "Rock")
            return true;

        }
        return false;
    }

    void Push()
    {
        if (DetectCollisions() == true && MindPower.currentHit == this.transform )
        {

            Vector3 hitDetectionPositionToMove = hitDetection.transform.position + transform.forward * speedPush * Time.deltaTime;
            hitDetection.collider.GetComponent<Rigidbody>().MovePosition(hitDetectionPositionToMove);
            hitDetection.collider.transform.Rotate(transform.right);
            //rb.MovePosition(hitDetectionPositionToMove);
            GetComponent<CharacterController>().speed = 1;
            detect = true;

        }

        else if (DetectCollisions() == false)
        {
            if (detect == true)
            {
                GetComponent<CharacterController>().speed = beginSpeed;
                detect = false;
            }
        }

    }
}
