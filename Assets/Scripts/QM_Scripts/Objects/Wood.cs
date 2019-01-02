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

    }

    void Ignite()
    {
        if (transform.parent != null && isBurning == false)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                isBurning = true;
                GameObject particleFires = Instantiate(Resources.Load("ParticleFire"), firePosition.position, Quaternion.identity) as GameObject;
                particleFires.transform.parent = this.transform;
                particleFires.transform.localScale = Vector3.one;
                gameObject.AddComponent<Ignitable>();

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

        if (other.gameObject.layer == LayerMask.NameToLayer("Entity"))
        {
            if (isBurning == true && other.gameObject.GetComponent<Ignitable>() == false)
            {
                isOtherBurning = true;
                GameObject particleFires = Instantiate(Resources.Load("ParticleFire"), other.collider.transform.position, Quaternion.identity) as GameObject;
                particleFires.transform.parent = other.transform;
                particleFires.transform.localScale = Vector3.one;
                particleFires.transform.rotation = Quaternion.Euler(new Vector3(270, 0, 0));
                other.gameObject.AddComponent<Ignitable>();
            }
        }
    }

}
