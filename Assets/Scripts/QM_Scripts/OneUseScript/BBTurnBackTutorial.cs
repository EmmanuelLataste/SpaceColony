using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BBTurnBackTutorial : MonoBehaviour {

    [SerializeField] GameObject player;
    [SerializeField] Text texts;
    bool firstTextOK;
    [SerializeField] MindPower mp;
    float beginTimerPossess;
    [SerializeField] string[] sentences;
    [SerializeField] AudioSource aS;
    bool isTriggered;
    private void Start()
    {
        //texts.text = "Hold Right Click / LT";
        texts.text = sentences[0];
        beginTimerPossess = MindPower.timerPossess;
    }

    private void Update()
    {
        TextChange();
        if (firstTextOK == true && MindPower.isMindManipulated == true)
        {
            MindPower.timerPossess = beginTimerPossess;
            Time.timeScale = 1;
            texts.enabled = false;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isTriggered == false)
             StartCoroutine(SoundIncrease());
        

    }

    IEnumerator SoundIncrease()
    {
        isTriggered = true;
        aS.Play();
        player.GetComponent<ObjectSound>().soundRadius = 20;
        yield return new WaitForSeconds(.1f);
        player.GetComponent<ObjectSound>().soundRadius = 0;
        yield return new WaitForSeconds(.75f);
        Time.timeScale = .5f;
        texts.enabled = true;
        firstTextOK = true;
        MindPower.timerPossess = 1.5f;
        yield return null;
    }

    void TextChange()
    {
        if (Input.GetButtonDown("Fire2") || Input.GetAxis("Fire2") > 0)
        {
            if (firstTextOK == true)
                //texts.text = "Aim the target and hold Left Clic / RT";
                texts.text = sentences[1];
        }
    }
}
