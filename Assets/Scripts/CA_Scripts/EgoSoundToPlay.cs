<<<<<<< HEAD
version https://git-lfs.github.com/spec/v1
oid sha256:b2ede7dbb12bbe97d4f04bd33d7ab5cdcbc5f5307f818bad44e4a07b2d0a4866
size 1153
=======
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EgoSoundToPlay : MonoBehaviour {

    [SerializeField]
    AudioManager audioManager;

    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    DialogManager dialogManager;

    [SerializeField]
    private bool triggered = false;

    [Header("The ID referencing the precise voice line to use")]
    [SerializeField]
    private int ID;

    public Dialog dialog;

    private void Update()
    {
        if (triggered == true)
        {
            if (!audioSource.isPlaying)
            {
                FindObjectOfType<DialogManager>().EndDialog();
                Destroy(this);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && triggered == false)
        {
            triggered = true;
            FindObjectOfType<DialogManager>().StartDialog(dialog);
            audioManager.EgoDialog(ID);
            audioSource.clip = audioManager.soundToPlay;
            audioSource.Play();
        }
    }

}
>>>>>>> Develop
