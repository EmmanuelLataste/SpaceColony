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
        if (transformPosition != null)
        {
            transform.position = transformPosition.transform.position;
        }

        else
        {
            Destroy(gameObject);
            Destroy(gameObject.transform.parent.gameObject);
        }
      
	}
}
