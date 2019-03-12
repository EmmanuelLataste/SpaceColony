using System.Collections;
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
