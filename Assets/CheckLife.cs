using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckLife : MonoBehaviour {
    [SerializeField] GameObject ccGO;
    [SerializeField] bool isAlive;
    // Use this for initialization
    void Start () {
        isAlive = ccGO.GetComponent<Life>().isAlive;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
