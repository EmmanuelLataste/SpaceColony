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
    bool isPushing;
    [SerializeField] float rushDamage;
    [SerializeField] float forceRush;
    CharacterController cc;
    Animator anim;
    Vector3 hitDetectionPositionToMove;
    FixedJoint fj;
    Vector3 positionToMove;
    [SerializeField]  float radiusPush;
    [SerializeField]  LayerMask maskPush;
    [SerializeField] Collider[] pushCollider;
    [SerializeField] Vector3 positionPush;
    [SerializeField] GameObject positionPushCollider;
    [SerializeField] float distance;
    GameObject pushedGO;
    private void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
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
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(positionPushCollider.transform.position, radiusPush);
    }

    void Push()
    {
        if (pushedGO != null) distance = Vector3.Distance(pushedGO.transform.position, transform.position);

        pushCollider =  Physics.OverlapSphere(positionPushCollider.transform.position, radiusPush, maskPush);

        if ( MindPower.currentHit == this.transform && pushCollider.Length != 0 && Input.GetAxisRaw("Vertical") > 0)
        {
            if (isPushing == false )
            {
                pushedGO = pushCollider[0].gameObject;
                anim.SetBool("Push", true);
                fj = gameObject.AddComponent<FixedJoint>();
                fj.connectedBody = pushCollider[0].GetComponent<Rigidbody>();
                fj.breakForce = 1000000;
                
                cc.smoothRotationPlayer = 0;
                
                isPushing = true;
                distance =  Vector3.Distance(pushCollider[0].transform.position, transform.position);
                pushCollider[0].transform.Find("Rock_Renderer").transform.rotation = transform.rotation;
            }
           
           
                cc.canMove = false;
                hitDetectionPositionToMove = pushCollider[0].transform.position + transform.forward * speedPush * Time.deltaTime;
                positionToMove = transform.position + transform.forward * speedPush * Time.deltaTime;
                pushCollider[0].GetComponent<Rigidbody>().MovePosition(hitDetectionPositionToMove);
               
            pushCollider[0].transform.Find("Rock_Renderer").transform.Rotate(-transform.forward);

            //rb.MovePosition(positionToMove);
            //pushCollider[0].transform.(transform.forward);
            //rb.MovePosition(PositionToMove);
            //cc.canMove = false;
            detect = true;
            
        }

        else if (distance >= 4 || isPushing == true && Input.GetAxisRaw("Vertical") == 0)
        {
                pushedGO = null;
                Destroy(fj);
                isPushing = false;
                cc.canMove = true;
                anim.SetBool("Push", false);
                detect = false;
                cc.smoothRotationPlayer = beginSmoothRotation;
            
        }

       
    }

    void Rush()

    {
        if (isRushing == true)
        {
            Debug.Log("Rush");
            anim.SetBool("Rush", true);
            rb.MovePosition(transform.position + Time.deltaTime * 20 * transform.forward);
            GetComponent<CharacterController>().speed = 0;
            GetComponent<CharacterController>().smoothRotationPlayer = 0.005f;
        }

        else
        {
            if (onceRushing == false)
            {
                anim.SetBool("Rush", false);
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
            anim.SetBool("Rush", true);
        }
    }
}
