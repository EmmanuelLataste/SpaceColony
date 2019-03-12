using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public Text nameText;   // UI Text object that displays the name of the speaking Entity (eg.EGO)
    public Text dialogText; // UI Text object that displays the dialogs of the speaking Entity

    [SerializeField]
    private float startDelay = 2.5f;    // Delay before the text starts to be displayed 
    [SerializeField]
    private float endDelay = 2f;    // Delay after which the text will disappear

    [Header("The bigger it is, the slower it gets")]
    [SerializeField]
    private float letterDisplayingSpeed = 0.05f;    // Delay in secs between each letter

    private Queue<string> sentences;    // Queue that holds the dialogs that will be displayed

    private Animator nameAnimator;
    private Animator dialogAnimator;

    private void Start()
    {
        sentences = new Queue<string>();
        nameAnimator = nameText.GetComponent<Animator>();
        dialogAnimator = dialogText.GetComponent<Animator>();
    }

    public void StartDialog(Dialog dialog)
    {
        nameAnimator.SetTrigger("dialogFadeInTrigger");
        dialogAnimator.SetTrigger("dialogFadeInTrigger");

        nameText.text = dialog.name;

        sentences.Clear();

        StartCoroutine(DialogStartDelay(dialog));
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialog();
            return;
        }

        string sentence = sentences.Dequeue();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(letterDisplayingSpeed);
        }
    }

    public void EndDialog()
    {
        StartCoroutine(DialogEndDelay());
    }

    IEnumerator DialogStartDelay(Dialog dialog)
    {
        yield return new WaitForSeconds(startDelay);

        foreach (string sentence in dialog.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    IEnumerator DialogEndDelay()
    {
        nameAnimator.SetTrigger("dialogFadeOutTrigger");
        dialogAnimator.SetTrigger("dialogFadeOutTrigger");
        yield return new WaitForSeconds(endDelay);
        dialogText.text = "";
        nameText.text = "";
        Debug.Log("Killed The Text");
    }
}
