using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flammable : MonoBehaviour {
    private bool isBurning = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Ignitable>() && isBurning == false)
        {
            isBurning = true;
            GameObject particleFires = Instantiate(Resources.Load("ParticleFire"), transform.position, Quaternion.identity) as GameObject;
            particleFires.transform.parent = this.transform;

        }
    }
}
