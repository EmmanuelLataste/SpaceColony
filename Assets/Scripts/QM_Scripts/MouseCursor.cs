using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour {

    Vector3 cursorPost;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        cursorPost = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = cursorPost;
	}
}
