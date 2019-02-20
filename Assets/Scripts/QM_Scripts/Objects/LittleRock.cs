using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleRock : MonoBehaviour {
    bool isPicked;
    [SerializeField] Transform transformObjectInHand;

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

            transform.position = transformObjectInHand.position;
            transform.rotation = transformObjectInHand.rotation;
            isPicked = true;

        }

        else if (transform.parent == false && isPicked == true)
        {
            isPicked = false;
            gameObject.GetComponent<ObjectSound>().FindReceivers();
        }
    }
}
