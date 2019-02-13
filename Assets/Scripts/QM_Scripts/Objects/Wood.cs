using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : Flammable
{
    private bool isOtherBurning;
    private GameObject particleFires;

    public Transform firePosition;
    public GameObject entity;
    bool isPicked;
    bool isStillBurning;


    [SerializeField] Transform transformObjectInHand;


    

    private void Update()
    {
        Ignite();
        if (isBurning == true)
        {
            gameObject.AddComponent<Ignitable>();
            isBurning = false;
        }
        if (transform.parent == true && isPicked == false)
        {
            PositionWhenPicked();
        }

        else if (transform.parent == false && isPicked == true)
        {
            isPicked = false;
            gameObject.GetComponent<ObjectSound>().FindReceivers();            
        }


    }

    void Ignite()
    {
        if (transform.parent != null && isBurning == false)
        {
            if (Input.GetButtonDown("X") || Input.GetKeyDown(KeyCode.R) && GetComponent<Ignitable>() == false)
            {

                isBurning = true;
                isStillBurning = true;

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

        float x1 = -90 - transform.eulerAngles.x;
        float y1 = 180 - transform.localEulerAngles.y;
        transform.position = transformObjectInHand.position;
        transform.rotation = transformObjectInHand.rotation;
        //transform.rotation =Quaternion.Euler(new Vector3(-84, 0, 180));

        //transform.parent.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        //transform.rotation = Quaternion.Euler(new Vector3(-294, 220, 0));
        isPicked = true;


    }

    private void OnTriggerEnter(Collider other)
    {
        if (transform.parent != null && other.gameObject.layer == LayerMask.NameToLayer("Entity") && other.GetComponent<Ignitable>() == false && isStillBurning == true )
        {
            if (other.GetComponent<CharacterController>().enabled == false || other.GetComponent<CharacterController>().otherGameObject != gameObject)
            {
                Debug.Log("TRU");
                other.GetComponent<CharacterController>().isBurning = true;
                other.gameObject.AddComponent<Ignitable>();
            }

        }

        if (other.gameObject.tag == "Cannister" && isStillBurning == true)
        {
            StartCoroutine(other.gameObject.GetComponent<Cannister>().Boom());
        }
    }
}

    


