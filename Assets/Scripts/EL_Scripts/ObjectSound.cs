using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSound : MonoBehaviour {

    private IEnumerator coroutine;
    public LayerMask receiversMask;
    public float soundRadius;

    [HideInInspector]
    public List<Transform> soundReceivers = new List<Transform>();

    private void Start() {
        coroutine = SoundSource();
    }


    private IEnumerator SoundSource() {
        gameObject.layer = 15;
        gameObject.tag = "SoundSource";
        yield return new WaitForSeconds(4f);
        gameObject.layer = 13;
        gameObject.tag = "Wood";
    }

    public void FindReceivers() {
        soundReceivers.Clear();
        Collider[] receiversInRadius = Physics.OverlapSphere(transform.position, soundRadius, receiversMask);

        if (receiversInRadius.Length > 0) {
            StartCoroutine(SoundSource());
        }
    }
}
