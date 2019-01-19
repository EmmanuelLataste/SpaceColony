using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flammable : MonoBehaviour {
    public bool isBurning = false;
    

    //public virtual void OnTriggerEnter(Collider other)
    //{
        
    //    if (other.GetComponent<Ignitable>() && isBurning == false)
    //    {
            
    //        isBurning = true;
    //        gameObject.AddComponent<Ignitable>();

    //    }
    //}

   void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<Ignitable>() && isBurning == false)
        {

            isBurning = true;
            gameObject.AddComponent<Ignitable>();

        }
    }

   
}
