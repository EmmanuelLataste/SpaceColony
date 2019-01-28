using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : Flammable{
    private bool isOtherBurning;
    private GameObject particleFires;
    public Transform firePosition;

    private void Update()
    {
        Ignite();
        if (isBurning == true)
        {
            gameObject.AddComponent<Ignitable>();
            isBurning = false;
        }

    }

    void Ignite()
    {
        if (transform.parent != null && isBurning == false)
        {
            if (Input.GetButtonDown ("X") && GetComponent<Ignitable>() == false)
            {
    
                isBurning = true;
                
            }
        }
    }



    private IEnumerator OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Wood")
        {
            yield return new WaitForSeconds(0.25f);
            //GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

       
    }

    

}
