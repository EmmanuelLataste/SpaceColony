using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : Flammable
{
    private bool isOtherBurning;
    //private GameObject particleFires;

    //public Transform firePosition;
    bool isPicked;



    [SerializeField] Transform transformObjectInHand;
     [SerializeField]Collider[] burnOthers;
    [SerializeField] float radiusBurn;
    [SerializeField] LayerMask maskBurn;


    private void Update()
    {
        //Ignite();
        //IgniteOthers();
        TurnFireOn();
        OnFire();
        if (transform.parent == true && isPicked == false)
        {
            PositionWhenPicked();
        }

        else if (transform.parent == false && isPicked == true)
        {
            isPicked = false;
            //gameObject.GetComponent<ObjectSound>().FindReceivers();            
        }


    }

    //void Ignite()
    //{
    //    if (transform.parent != null && isBurning == false)
    //    {
    //        if (Input.GetButtonDown("X") || Input.GetKeyDown(KeyCode.R) && GetComponent<Ignitable>() == false)
    //        {

    //            isBurning = true;
    //            isStillBurning = true;

    //        }
    //    }
    //}

    private IEnumerator OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Wood")
        {
            yield return new WaitForSeconds(0.25f);
            //GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }


    }

    void TurnFireOn()
    {
        if (Input.GetKeyDown(KeyCode.R) && isPicked == true)
        {
            isBurning = true;
        }
    }

    void PositionWhenPicked()
    {

      
        transform.position = transformObjectInHand.position;
        transform.rotation = transformObjectInHand.rotation;
      
        isPicked = true;


    }


    //void IgniteOthers()
    //{
    //    if (isStillBurning == true && transform.parent != null)
    //    {
    //        burnOthers = Physics.OverlapSphere(transform.position, radiusBurn, maskBurn);

    //        foreach (Collider collid in burnOthers)
    //        {
    //            if (collid.GetComponent<Ignitable>() == false && collid.gameObject.tag != "Player" && collid.gameObject.layer == LayerMask.NameToLayer("Entity") )
    //            {
    //                Debug.Log("TRU");
    //                collid.GetComponent<CharacterController>().isBurning = true;
    //                collid.gameObject.AddComponent<Ignitable>();
    //            }

    //            else if (collid.gameObject.layer == LayerMask.NameToLayer("Explosive"))
    //            {
    //                StartCoroutine(collid.gameObject.GetComponent<Explosive>().Boom());
    //            }



    //        }
    //    }
    //}


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (transform.parent != null && other.gameObject.layer == LayerMask.NameToLayer("Entity") && other.GetComponent<Ignitable>() == false && isStillBurning == true )
    //    {
            
    //            Debug.Log("TRU");
    //            other.GetComponent<CharacterController>().isBurning = true;
    //            other.gameObject.AddComponent<Ignitable>();
            

    //    }

    //    if (other.gameObject.tag == "Cannister" && isStillBurning == true)
    //    {
    //        StartCoroutine(other.gameObject.GetComponent<Cannister>().Boom());
    //    }
    //}

    
}

    


