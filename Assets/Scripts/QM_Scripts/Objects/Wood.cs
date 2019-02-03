using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : Flammable {
    private bool isOtherBurning;
    private GameObject particleFires;
    public Transform firePosition;
    public GameObject entity;

    private void Update()
    {
        Ignite();
        if (isBurning == true)
        {
            gameObject.AddComponent<Ignitable>();
            isBurning = false;
        }
        PositionWhenPicked();

    }

    void Ignite()
    {
        if (transform.parent != null && isBurning == false)
        {
            if (Input.GetButtonDown ("X") || Input.GetKeyDown(KeyCode.R) && GetComponent<Ignitable>() == false)
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

    void PositionWhenPicked()
    {
        if (transform.parent == true)
        {
            transform.rotation =Quaternion.Euler(new Vector3(-84, 0, 180));
        }
    }

    

}
