using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationPlayer : MonoBehaviour {
    public GameObject targetCam;
    private float smoothRotationPlayer;
    [Range(0, 15)]
    public float smoothRotationPlayerTimer;

    void Update () {
        Rotation();
	}

    void Rotation()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetCam.transform.rotation, smoothRotationPlayer);
            smoothRotationPlayer += smoothRotationPlayerTimer * Time.deltaTime;
        }

        else if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            smoothRotationPlayer = 0;
        }
           
        
    }
}
