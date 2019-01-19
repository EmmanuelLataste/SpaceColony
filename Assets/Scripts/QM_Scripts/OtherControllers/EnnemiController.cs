using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiController : CharacterController {



    private void Awake()
    {
        
        targetRotationCam = GameObject.Find("TargetRotationCAM");
        cam = GameObject.Find("Cam_Normal");
        hangingObjectPosition = this.transform.Find("HangingObjectsPosition").gameObject;
        
    }

}

