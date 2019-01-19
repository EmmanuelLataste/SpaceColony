using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour {

    
    Vector3 littleOffset;
    Vector3 bigOffset;
    public bool isBoom;
    public Collider[] littleCircle;
    public Collider[] bigCircle;
    public float radiusHighDamage;
    public float radiusLowDamage;
    public float timer;
    public float timer2 = .5f;
    public GameObject explosionPrefab;

    void Update()
    {
        

    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Ignitable>() == true && isBoom == false)
        {
            Boom();
        }
    }

    public void OnDrawGizmos()
    {
        if (isBoom == true)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, radiusHighDamage);
            if (Time.time >= timer)
            {

                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(transform.position, radiusLowDamage);
            }

            if (Time.time >= timer + 1)
            {

                isBoom = false;
            }

        }

    }

    public void Boom()
    {

        littleCircle = Physics.OverlapSphere(transform.position, radiusHighDamage);
        bigCircle = Physics.OverlapSphere(transform.position, radiusLowDamage);
        InTheBoom();

        isBoom = true;
        timer = Time.time + timer2;
        Destroy(GetComponent<MeshRenderer>());
        GetComponent<Rigidbody>().isKinematic = true;
        GameObject explosion = Instantiate(explosionPrefab) as GameObject;
        explosion.transform.position = transform.position;
        Destroy(gameObject);
    }

    public void InTheBoom()
    {
       foreach(Collider deadCollid in littleCircle)
        {

            if (deadCollid.gameObject.layer == LayerMask.NameToLayer("Entity") && deadCollid is CapsuleCollider)
            {
                deadCollid.GetComponent<Life>().healthPoints -= 50;
                
            }
        }

        foreach (Collider hurtCollid in bigCircle)
        {
            if (hurtCollid.gameObject.layer == LayerMask.NameToLayer("Entity") && hurtCollid is CapsuleCollider)
            {
               
                hurtCollid.GetComponent<Life>().healthPoints -= Mathf.Round((-3 * Vector3.Distance(transform.position, hurtCollid.transform.position) + 30) * 10) / 10 ;
                // fonction affine après calcul par rapport de la distance x de 5 à 10 et f(x) de 15 à 0.
            }

        }
    }





}
