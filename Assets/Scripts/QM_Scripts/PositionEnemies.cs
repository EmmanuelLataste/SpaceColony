using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionEnemies : MonoBehaviour {

    
    public GameObject transformPosition;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (transformPosition != null && MindPower.isMindManipulated == false)
        {
            transform.position = transformPosition.transform.position;
            transform.rotation = transformPosition.transform.rotation;
        }

        else if (transformPosition != null && MindPower.isMindManipulated == true)
        {
            transformPosition.transform.position = transform.position;
            transformPosition.transform.rotation = transform.rotation;
        }

        else
        {
            Destroy(gameObject);
            Destroy(gameObject.transform.parent.gameObject);
        }
      
	}
}
