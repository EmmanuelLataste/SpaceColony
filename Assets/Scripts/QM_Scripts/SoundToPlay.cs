using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundToPlay : MonoBehaviour {

    Animator anim;
    AudioSource aS;
    [SerializeField] AudioManager am;
    [SerializeField] bool[] animStateInfo;


    // Use this for initialization
    void Start () {
        anim = GetComponent<PositionEnemies>().transformPosition.GetComponent<Animator>();
        aS = GetComponent<AudioSource>();
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (anim != null)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Patrolling") == true ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") == true)
            {
                SoundComparedToAnim(0, "Roaming");
;          
            }

            else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Chase"))
            {
                SoundComparedToAnim(1, "Chasing");

            }

            else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Investigating"))
            {
                SoundComparedToAnim(2, "Investigating");

            }

        }
    }
		

    void SoundComparedToAnim(int animStateInf, string typeOfSoundComparedToAnim)
    {
        if (animStateInfo[animStateInf] == false)
        {
            aS.Stop();
            for (int i = 0; i < animStateInfo.Length; i++)
            {
                if (animStateInfo[i] != animStateInfo[animStateInf])
                {
                    animStateInfo[i] = false;
                }
            }
            animStateInfo[animStateInf] = true;


        }


        if (aS.isPlaying == false && animStateInfo[animStateInf] == true)
        {
            am.PlaySound(typeOfSoundComparedToAnim);
            aS.clip = am.soundToPlay;
            aS.Play();
        }


    }
}
