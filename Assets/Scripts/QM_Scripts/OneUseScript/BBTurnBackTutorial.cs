using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBTurnBackTutorial : MonoBehaviour {

    [SerializeField] GameObject bigBrainless;

	

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            bigBrainless.transform.LookAt(other.transform);
        }
    }
}
