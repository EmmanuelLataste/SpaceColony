using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruits : Goo {

    bool canExplose;
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        PositionWhenPicked();

        if (transform.parent != null) canExplose = true;
    }


    void PositionWhenPicked()
    {
        if (transform.parent == true)
        {
            transform.rotation = Quaternion.Euler(new Vector3(-84, 0, 180));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

            if (canExplose == true)
        {
            GooToGround();
            StartCoroutine(StopGoo());
            
        }

    }
}


