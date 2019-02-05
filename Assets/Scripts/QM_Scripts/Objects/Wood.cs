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
        }


    }

    void Ignite()
    {
        if (transform.parent != null && isBurning == false)
        {
            if (Input.GetButtonDown("X") || Input.GetKeyDown(KeyCode.R) && GetComponent<Ignitable>() == false)
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

        float x1 = -90 - transform.eulerAngles.x;
        float y1 = 180 - transform.localEulerAngles.y;

        //transform.rotation =Quaternion.Euler(new Vector3(-84, 0, 180));
        transform.parent.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        transform.rotation = Quaternion.Euler(new Vector3(-294, 220, 0));
        isPicked = true;


    }
}

    


