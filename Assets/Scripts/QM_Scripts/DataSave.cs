using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataSave{
    public float[] position;
	

    public DataSave(CharacterController playerController)
    {
        position = new float[3];
        position[0] = playerController.transform.position.x;
        position[1] = playerController.transform.position.y;
        position[2] = playerController.transform.position.z;
    }
}
