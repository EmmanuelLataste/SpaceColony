using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour {

    public float healthPoints;
    bool isAlive = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Dead();

        if (healthPoints <= 0)
        {
            isAlive = false;
        }
    }


    void Dead()
    {
        if (isAlive == false)
        {

            gameObject.SetActive(false);

        }
    }
}
