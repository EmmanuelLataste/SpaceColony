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
    int newSpeed;
    GameObject[] gooToThingObjetcs;
    MeshRenderer mr;


    private void Update()
    {
        
    }

    public void GooToGround()
    {
        Collider[] collidGround = Physics.OverlapSphere(transform.position, radiusColliderGoo);
        foreach (Collider gooCollid in collidGround)
        {
            if (gooCollid.gameObject.layer == LayerMask.NameToLayer("Entity"))
            {
                if (MindPower.currentHit != null && MindPower.currentHit.transform == gooCollid.transform || gooCollid.gameObject.tag == "Player")
                    gooCollid.GetComponent<CharacterController>().speed = newSpeed;

                else gooCollid.GetComponent<PositionEnemies>().transformPosition.GetComponent<NavMeshAgent>().speed = newSpeed;

            }

            if (gooCollid.gameObject.tag == "Cannister")
            {
                //for (int i = 0; i < collidGround.Length; i++)
                //{
                //    if (Vector3.Distance(gooCollid.transform.position, collidGround[i].transform.position) < 2
                //        && collidGround[i].gameObject.layer == LayerMask.NameToLayer("Entity") && collidGround[i] is CapsuleCollider)
                //    {
                //        Debug.Log(collidGround[i].name);

                //        if (MindPower.currentHit != null && MindPower.currentHit.transform == collidGround[i].transform || collidGround[i].gameObject.tag == "Player")
                //            collidGround[i].transform.parent = gooCollid.transform;

                //        else
                //        {
                //            collidGround[i].transform.parent.parent = gooCollid.transform;

                //            collidGround[i].GetComponent<PositionEnemies>().enabled = false;
                //        }
                //        collidGround[i].GetComponent<CharacterController>().enabled = false;


                //    }
                //}

                StartCoroutine(gooCollid.GetComponent<Cannister>().Fly(1, 1.3f));
            }
        }

        
    }

    public IEnumerator StopGoo()
    {
        yield return new WaitForSeconds(timeSlow);
        Collider[] collidGround = Physics.OverlapSphere(transform.position, radiusColliderGoo);
        foreach (Collider gooCollid in collidGround)
        {
            if (gooCollid.gameObject.layer == LayerMask.NameToLayer("Entity"))
            {
                if (MindPower.currentHit != null && MindPower.currentHit.transform == gooCollid.transform || gooCollid.gameObject.tag == "Player")
                    gooCollid.GetComponent<CharacterController>().speed = gooCollid.GetComponent<CharacterController>().beginSpeed;

                else gooCollid.GetComponent<PositionEnemies>().transformPosition.GetComponent<NavMeshAgent>().speed =
                     gooCollid.GetComponent<CharacterController>().beginSpeed;

            }
        }
        Destroy(this.gameObject);
        yield return null;
    }

   

}