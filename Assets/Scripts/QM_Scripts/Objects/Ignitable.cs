using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ignitable : MonoBehaviour {

    //bool isBurning;
    //public bool isBurnable = true;
    //float timer;
    //float initialTimer;
    //float timerStopFire = 6;
    //float timerSF;
    //float life;
    //bool isOnContact;
    //[SerializeField] GameObject firePosition;
    //public GameObject particleFires;
    //[SerializeField] Collider[] fireCollider;
    //[SerializeField] float radiusFire = 2;
    //bool isStillBurning;
    //Flammable flammable;
    

    //private void Start()
    //{
    //    particleFires = Instantiate(Resources.Load("ParticleFire"), firePosition.transform.position, Quaternion.identity) as GameObject;
    //    particleFires.transform.parent = this.transform;
    //    particleFires.transform.localScale = Vector3.one;
    //    flammable = GetComponent<Flammable>()
       
    //    //isBurning = true;

    //}

    //void Update()
    //{
    //    isStillBurning = flammable.isStillBurning;
    //    //OnFire();


    //}

  

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.layer == LayerMask.NameToLayer("Entity"))
    //    {
    //        collision.gameObject.GetComponent<Flammable>().isBurning = true;
    //    }
    //}

    //private void OnTriggerStay(Collider other)
    //{
    //    if ( other.GetComponent<Ignitable>() == true)
    //    {
    //        isOnContact = true;
    //    }
    //}

    //public virtual void OnTriggerExit(Collider other)
    //{
    //        isOnContact = false;
    //}



    ////public void OnFire()
    ////{
    ////    if (isBurnable == true)
    ////    {
    ////        timerSF = Time.time + timerStopFire;
    ////        timer = Time.time + 1f;
    ////        initialTimer = Time.time;
    ////        isBurnable = false;

    ////    }

    ////    else if ( isBurnable == false && GetComponent<Life>() == true && timerSF > Time.time)
    ////    {

    ////        if (Time.time >= timer && initialTimer + 3.5f >= Time.time && isOnContact == false)
    ////        {
                
    ////            GetComponent<Life>().Damages(.25f);
    ////            timer = Time.time + 1f;
    ////        }

    ////        else if (Time.time >= timer && initialTimer + 3.5f < Time.time && isOnContact == false)
    ////        {
    ////            GetComponent<Life>().Damages(.5f);
    ////            timer = Time.time + 1f;
    ////        }

    ////        else if (Time.time >= timer && isOnContact == true)
    ////        {
    ////            GetComponent<Life>().Damages(.75f);
    ////            timer = Time.time + 1f;
    ////        }
    ////    }

    ////    else if (timerSF <= Time.time && isBurning == true && gameObject.tag != "Wood")
    ////    {
    ////        GetComponent<Flammable>().isBurning = false;
    ////        Destroy(GetComponent<Ignitable>());
    ////        Destroy(particleFires);
    ////        isBurnable = true;
    ////    }
    ////}


}
