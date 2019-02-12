using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ignitable : MonoBehaviour {

    bool isBurning;
    public bool isBurnable = true;
    float timer;
    float initialTimer;
    float life;
    bool isOnContact;
    GameObject firePosition;
    public GameObject particleFires;


    void Start()
    {
        if (transform.Find("FirePosition") == true)
        {
            firePosition = transform.Find("FirePosition").gameObject;
        }

            if (isBurning == false)
        {
            if (transform.Find("FirePosition") == true)
            {
                particleFires = Instantiate(Resources.Load("ParticleFire"), firePosition.transform.position, Quaternion.identity) as GameObject;
            }

            else
            {
                particleFires = Instantiate(Resources.Load("ParticleFire"), transform.position, Quaternion.identity) as GameObject;
               
            }

            particleFires.transform.parent = this.transform;
            particleFires.transform.localScale = Vector3.one;
            isBurning = true;
        }

    }

    void Update()
    {
        OnFire();
    }

    private void OnTriggerStay(Collider other)
    {
        if ( other.GetComponent<Ignitable>() == true)
        {
            isOnContact = true;
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
            isOnContact = false;
    }

    public void OnFire()
    {
        if (isBurnable == true)
        {
            
            timer = Time.time + 1f;
            initialTimer = Time.time;
            isBurnable = false;

        }

        if ( isBurnable == false && GetComponent<Life>() == true)
        {

            if (Time.time >= timer && initialTimer + 3.5f >= Time.time && isOnContact == false)
            {
                
                GetComponent<Life>().Damages(.25f);
                timer = Time.time + 1f;
            }

            else if (Time.time >= timer && initialTimer + 3.5f < Time.time && isOnContact == false)
            {
                GetComponent<Life>().Damages(.5f);
                timer = Time.time + 1f;
            }

            else if (Time.time >= timer && isOnContact == true)
            {
                GetComponent<Life>().Damages(.75f);
                timer = Time.time + 1f;
            }
        }
    }


}
