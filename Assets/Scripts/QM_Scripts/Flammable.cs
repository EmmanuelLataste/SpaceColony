using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flammable : MonoBehaviour {
    
    protected virtual void Burn()
    {
        GameObject particles = Instantiate(Resources.Load("ParticleFire"), transform.position, Quaternion.identity) as GameObject;
        particles.transform.parent = transform;
        
    }
}
