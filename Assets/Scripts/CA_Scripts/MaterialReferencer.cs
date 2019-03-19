<<<<<<< HEAD
version https://git-lfs.github.com/spec/v1
oid sha256:b8acdf8b1af95cd915962fdbcc99dff263c895c85085dd07843cdc826a8a405e
size 415
=======
﻿using System.Collections;
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
>>>>>>> Develop
