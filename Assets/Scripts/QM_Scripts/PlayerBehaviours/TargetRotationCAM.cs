using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRotationCAM : MonoBehaviour {
    private float rotation;
    

	void Update () {

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            rotation = (Mathf.Atan2(-Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")) * Mathf.Rad2Deg);

            transform.localRotation = Quaternion.Euler(new Vector3(0, rotation + 90, 0));
        }
        
    }
}
