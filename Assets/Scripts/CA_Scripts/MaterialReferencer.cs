using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialReferencer : MonoBehaviour {


    public Material mat;
    public string pos = "_PlayerPosition";
    public Transform player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        mat.SetVector(pos, player.position);
	}
}
