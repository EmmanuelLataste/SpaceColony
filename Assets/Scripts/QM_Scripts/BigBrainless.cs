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
    float beginSmoothRotation;
    bool detect;
    Rigidbody rb;
    bool isRushing;
    bool onceRushing;
    [SerializeField] float rushDamage;
    [SerializeField] float forceRush;

    private void Start()
    {

        rb = GetComponent<Rigidbody>();
        beginSpeed = GetComponent<CharacterController>().beginSpeed;
        speed = GetComponent<CharacterController>().speed;
        beginSmoothRotation = GetComponent<CharacterController>().smoothRotationPlayer;
    }

    private void FixedUpdate()
    {
        Push();
        Rush();
        if (Input.GetKeyDown(KeyCode.G) || Input.GetButtonDown("Fire3"))
        {
            if (MindPower.currentHit == transform && isRushing == false)
            isRushing = true;
        }

        else if(Input.GetKeyDown(KeyCode.G) || Input.GetButtonDown("Fire3"))
        {
            if (MindPower.currentHit == transform && isRushing == true)
            {
                isRushing = false;
                onceRushing = false;
            }

        }
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
        if (DetectCollisions() == true && MindPower.currentHit == this.transform)
        {

            Vector3 hitDetectionPositionToMove = hitDetection.transform.position + transform.forward * speedPush * Time.deltaTime;
            hitDetection.collider.GetComponent<Rigidbody>().MovePosition(hitDetectionPositionToMove);
            hitDetection.collider.transform.Rotate(transform.right);
            //rb.MovePosition(hitDetectionPositionToMove);
            GetComponent<CharacterController>().speed = 2;
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

    void Rush()

    {
        if (isRushing == true)
        {
            Debug.Log("Rush");
            rb.MovePosition(transform.position + Time.deltaTime * 20 * transform.forward);
            GetComponent<CharacterController>().speed = 0;
            GetComponent<CharacterController>().smoothRotationPlayer = 0.005f;
        }

        else
        {
            if (onceRushing == false)
            {

                GetComponent<CharacterController>().speed = beginSpeed;
                GetComponent<CharacterController>().smoothRotationPlayer = beginSmoothRotation;
                onceRushing = true;
            }

        }
            

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Entity") && isRushing == true)
        {
            onceRushing = false;
            Debug.Log("colll");
            other.gameObject.GetComponent<Life>().Damages(rushDamage);
            other.gameObject.GetComponent<Rigidbody>().AddForce( transform.forward * Time.deltaTime * forceRush * 100000);
            isRushing = false;
        }
    }
}
