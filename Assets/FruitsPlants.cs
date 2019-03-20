using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitsPlants : MonoBehaviour {


    bool isFruit;
    [SerializeField] GameObject fruitSpawn;
    [SerializeField] GameObject transformObjectInHand;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isFruit == false)
        {
            fruitSpawn = Instantiate(Resources.Load("Fruits"), transform.position, Quaternion.identity) as GameObject;
            fruitSpawn.GetComponent<Fruits>().transformObjectInHand = transformObjectInHand.transform; 
            isFruit = true;
        }

        if (fruitSpawn == null)
        {
            isFruit = false;
        }
	}
}
