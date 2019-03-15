using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruits : Goo {

    bool canExplose;
    bool canExplose2;
    Rigidbody rb;
    bool isPicked;
    [SerializeField] Transform transformObjectInHand;
    [SerializeField] LayerMask[] maskGround;
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

            transform.position = transformObjectInHand.position;
            transform.rotation = transformObjectInHand.rotation;
            isPicked = true;
            
        }

        else if (transform.parent == false && isPicked == true)
        {
            isPicked = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (canExplose == true )
        {
           
            GooToGround();
            StartCoroutine(StopGoo());
            Debug.Log("collision");
            canExplose = false;
                
        }

    }

  
}


