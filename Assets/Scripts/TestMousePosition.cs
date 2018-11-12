using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMousePosition : MonoBehaviour {
    Camera cam;
	// Use this for initialization
	void Start () {
        cam.GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
	}
}
