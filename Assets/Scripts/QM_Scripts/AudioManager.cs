<<<<<<< HEAD
version https://git-lfs.github.com/spec/v1
oid sha256:5a7376233ffdc7e377d0e2af86931321b751fa427fe1fd172335b4a23a4b4c17
size 2483
=======
﻿using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [SerializeField] AudioClip[] LB_Roaming;
    [SerializeField] AudioClip[] LB_Chasing;
    [SerializeField] AudioClip[] LB_Investigating;
    [SerializeField] public AudioClip[] steps;
    [SerializeField] AudioClip[] Ego_Narration;
    public AudioClip soundToPlay;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySound(string type)
    {
        if (type == "Roaming")
        {
            int randomSound = Random.Range(0, LB_Roaming.Length);
            soundToPlay = LB_Roaming[randomSound];
        }

        else if (type == "Chasing")
        {
            int randomSound = Random.Range(0, LB_Chasing.Length);
            soundToPlay = LB_Chasing[randomSound];
        }

        else if (type == "Investigating")
        {
            int randomSound = Random.Range(0, LB_Investigating.Length);
            soundToPlay = LB_Investigating[randomSound];
        }

       
    }

    public void EgoDialog(int ID)
    {
        soundToPlay = Ego_Narration[ID];
    }
}


>>>>>>> Develop
