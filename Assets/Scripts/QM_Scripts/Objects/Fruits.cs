using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruits : Goo {

    bool canExplose;
    Rigidbody rb;
    bool isPicked;
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
        if (transform.parent == true && isPicked == false)
        {
            transform.rotation = Quaternion.Euler(new Vector3(-84, 0, 180));
            transform.position += transform.parent.right - new Vector3(.8f,0,0) ;
            isPicked = true;
            
        }

        else if (transform.parent == false && isPicked == true)
        {
            isPicked = false;
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


