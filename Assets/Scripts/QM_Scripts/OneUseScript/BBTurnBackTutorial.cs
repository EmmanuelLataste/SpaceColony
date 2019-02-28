using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBTurnBackTutorial : MonoBehaviour {

    [SerializeField] GameObject player;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(SoundIncrease());

    }

    IEnumerator SoundIncrease()
    {
        player.GetComponent<ObjectSound>().soundRadius = 20;
        yield return new WaitForEndOfFrame();
        player.GetComponent<ObjectSound>().soundRadius = 0;
        Destroy(gameObject);
    }
}
