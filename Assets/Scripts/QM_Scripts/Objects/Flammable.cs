using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flammable : MonoBehaviour {
    public bool isBurning = false;
    public bool isBurnable = true;
    float timer;
    float initialTimer;
    float life;
    bool isOnContact;


    public virtual void Update()
    {
        OnFire();
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Ignitable>() && isBurning == false)
        {
            isBurning = true;
            GameObject particleFires = Instantiate(Resources.Load("ParticleFire"), transform.position, Quaternion.identity) as GameObject;
            particleFires.transform.parent = this.transform;
            particleFires.transform.localScale = Vector3.one;
            gameObject.AddComponent<Ignitable>();

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isBurning == true)
        {
            isOnContact = true;
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if (isBurning == true)
        {
            isOnContact = false;
        }
    }

    public void OnFire()
    {
        if ( isBurnable == true && isBurning == true)
        {
            life = GameObject.FindGameObjectWithTag("Player").GetComponent<Life>().healthPoints;
            timer = Time.time + 1f;
            initialTimer = Time.time;
            isBurnable = false;
           
        }

        if (isBurning == true && isBurnable == false)
        {

            if (Time.time >= timer && initialTimer + 3.5f >= Time.time && isOnContact == false )
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<Life>().healthPoints -= .25f;
                timer = Time.time + 1f;
            }

            else if (Time.time >= timer && initialTimer + 3.5f < Time.time && isOnContact == false)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<Life>().healthPoints -= .5f;
                timer = Time.time + 1f;
            }

            else if (Time.time >= timer && isOnContact == true)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<Life>().healthPoints -= .75f;
                timer = Time.time + 1f;
            }
        }
    }
}
