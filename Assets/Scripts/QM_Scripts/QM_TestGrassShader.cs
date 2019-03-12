using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QM_TestGrassShader : MonoBehaviour {
    public Material grassMat;
    string posPlayer = "_PlayerPosition";
    public GameObject player;
    public Texture grassTexture;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //grassMat.SetVector(posPlayer, player.transform.position);
	}
}
