using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Goo : MonoBehaviour {


    bool isBoom;
    public Collider[] circle;
    public float radiusColliderGoo;
    float timer;
    public float timeSlow;
    public bool isGooAble;

    private void Update()
    {
        if (isGooAble == true)
        {
            InTheGooEnemyControlled();
            InTheGooEnemyNonControlled();
            InTheGooPlayer();
        }

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (isGooAble == true)
        {
            if (collision.gameObject.layer != LayerMask.NameToLayer("Entity"))
            {
                circle = Physics.OverlapSphere(transform.position, radiusColliderGoo);
                timer = Time.time + timeSlow;
                
                
                isBoom = true;
                Debug.Log("GOOOO");
                timer = Time.time + timeSlow;
            }



        }
    }

    private void OnDrawGizmos()
    {
        if (isBoom == true && Time.time < timer)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 5f);
        }

    }

    void InTheGooPlayer()
    {
        foreach (Collider gooCollid in circle)
        {

            if (gooCollid.gameObject.layer == LayerMask.NameToLayer("Entity") && gooCollid is CapsuleCollider)
            {

                if (gooCollid.gameObject.tag == "Player")
                {
                    if (Time.time < timer)
                    {
                        gooCollid.GetComponent<CharacterController>().speed = 0;
                        Debug.Log(gooCollid.GetComponent<CharacterController>().speed);
                    }

                    else
                    {
                        gooCollid.GetComponent<CharacterController>().speed = gooCollid.GetComponent<CharacterController>().beginSpeed;
                        Debug.Log(gooCollid.GetComponent<CharacterController>().speed);
                    }

                }
               
            }
        }
      
    }

    void InTheGooEnemyControlled()
    {
        foreach (Collider gooCollid in circle)
        {

            if (gooCollid.gameObject.layer == LayerMask.NameToLayer("Entity") && gooCollid is CapsuleCollider)
            {
                if (gooCollid.transform == MindPower.currentHit)
                {
                    if (Time.time < timer)
                    {
                        Debug.Log("MM");
                        gooCollid.GetComponent<EnnemiController>().speed = 0;
                    }

                    else
                    {
                        gooCollid.GetComponent<EnnemiController>().speed = gooCollid.GetComponent<EnnemiController>().beginSpeed;
                        Debug.Log(gooCollid.GetComponent<CharacterController>().speed);

                    }
                }

               

            }
        }
    }

    void InTheGooEnemyNonControlled()
    {
        foreach (Collider gooCollid in circle)
        {

            if (gooCollid.gameObject.layer == LayerMask.NameToLayer("Entity") && gooCollid is CapsuleCollider)
            {
                if (gooCollid.transform != MindPower.currentHit && gooCollid.gameObject.tag != "Player")
                {
                    if (Time.time < timer)
                    {
                        Debug.Log("Enemy");
                        gooCollid.GetComponent<NavMeshAgent>().speed = 0;
                    }

                    else
                    {
                        gooCollid.GetComponent<NavMeshAgent>().speed = gooCollid.GetComponent<EnnemiController>().beginSpeed;
                        Debug.Log(gooCollid.GetComponent<CharacterController>().speed);

                    }
                }
                
            }
        }

    }
}