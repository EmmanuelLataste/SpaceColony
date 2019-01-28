using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Goo : MonoBehaviour {

    bool isBoom;
    public Collider[] circle;
    public float radiusColliderGoo;
    float timer;
    float timerGizmo = .5f;
    public float timeSlow;
    public bool isGooAble;
    float timerBeforeDestroy;

    GameObject[] gooToThingObjetcs;
    MeshRenderer mr;

    Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        mr = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (isGooAble == false && Time.time >= timer)
        {
            isGooAble = true;
        }

        if (Time.time >= timer)
        {
            InTheGooEnemyControlled();
            InTheGooEnemyNonControlled();
            InTheGooPlayer();
            

        }

        GooToThings();
        

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (isGooAble == true)
        {
            if (collision.gameObject.layer != LayerMask.NameToLayer("Entity"))
            {
                circle = Physics.OverlapSphere(transform.position, radiusColliderGoo);
                timer = Time.time + timeSlow;
                timerGizmo = Time.time + .5f;
                isBoom = true;
                InTheGooEnemyControlled();
                InTheGooEnemyNonControlled();
                InTheGooPlayer();
                isGooAble = false;
                Destroy(mr);
                Destroy(gameObject, timeSlow +.5f);

            }



        }
    }

    private void OnDrawGizmos()
    {
        if (isBoom == true && Time.time <  timerGizmo)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 5f);
        }

    }

    void InTheGooPlayer()
    {
        foreach (Collider gooCollid in circle)
        {

            if (gooCollid != null && gooCollid.gameObject.layer == LayerMask.NameToLayer("Entity") && gooCollid is CapsuleCollider)
            {

                if (gooCollid.gameObject.tag == "Player")
                {
                    if (Time.time < timer)
                    {
                        gooCollid.GetComponent<CharacterController>().speed = 0;

                    }

                    else
                    {
                        gooCollid.GetComponent<CharacterController>().speed = gooCollid.GetComponent<CharacterController>().beginSpeed;
                        

                    }

                }
               
            }
        }
      
    }

    void InTheGooEnemyControlled()
    {
        foreach (Collider gooCollid in circle)
        {

            if (gooCollid != null && gooCollid.gameObject.layer == LayerMask.NameToLayer("Entity") && gooCollid is CapsuleCollider)
            {
                if (gooCollid.transform == MindPower.currentHit)
                {
                    if (Time.time < timer)
                    {
                        
                        gooCollid.GetComponent<EnnemiController>().speed = 0;
                    }

                    else
                    {
                        gooCollid.GetComponent<EnnemiController>().speed = gooCollid.GetComponent<EnnemiController>().beginSpeed;
                        

                    }
                }

               

            }
        }
    }

    void InTheGooEnemyNonControlled()
    {
        foreach (Collider gooCollid in circle)
        {

            if (gooCollid != null && gooCollid.gameObject.layer == LayerMask.NameToLayer("Entity") && gooCollid is CapsuleCollider)
            {
                if (gooCollid.GetComponent<NavMeshAgent>() != null && gooCollid.transform != MindPower.currentHit && gooCollid.gameObject.tag != "Player")
                {
                    if (Time.time < timer)
                    {

                        gooCollid.GetComponent<NavMeshAgent>().speed = 0;
                    }

                    else
                    {
                        gooCollid.GetComponent<NavMeshAgent>().speed = gooCollid.GetComponent<EnnemiController>().beginSpeed;
                        

                    }
                }
                
            }
        }

    }

    void GooToThings()
    {
        foreach (Collider gooCollid in circle)
        {

            if (gooCollid != null && gooCollid.CompareTag("Cannister") == true)
            {
                for (int i = 0; i < circle.Length; i++)
                {
                    
                    if (circle[i].gameObject.layer ==LayerMask.NameToLayer("Entity"))
                    {
                        
                        if (Vector3.Distance(gooCollid.transform.position, circle[i].transform.position) <= 2)
                        {
                           

                            gooCollid.GetComponent<Cannister>().isFlying = true;
                            
                            circle[i].transform.parent.position = gooCollid.transform.position + new Vector3(1, 0, 1);
                            circle[i].transform.parent.parent = gooCollid.transform;
                            Rigidbody rbI = circle[i].GetComponent<Rigidbody>();
                            rbI.useGravity = false;
                            rbI.detectCollisions = false;
                            rbI.constraints = RigidbodyConstraints.FreezeAll;
                            if (circle[i].GetComponent<PositionEnemies>() == true)
                            {
                                Destroy(circle[i].GetComponent<PositionEnemies>());
                            }

                            //circle[i].GetComponent<Rigidbody>().detectCollisions = false;
                            //circle[i].GetComponent<Rigidbody>().useGravity= false;
                            //foreach (Collider collider in circle[i].GetComponents<Collider>())
                            //{
                            //    Destroy(collider);
                            //}



                        }
                    }
                }

            }
        }

    }
}