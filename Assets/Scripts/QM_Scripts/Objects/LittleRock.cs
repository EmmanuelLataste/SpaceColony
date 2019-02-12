using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleRock : MonoBehaviour {
    bool isPicked;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        PositionWhenPicked();
	}

    void PositionWhenPicked()
    {
        if (transform.parent == true && isPicked == false)
        {


            isPicked = true;

        }

        else if (transform.parent == false && isPicked == true)
        {
            isPicked = false;
        }
    }
}
