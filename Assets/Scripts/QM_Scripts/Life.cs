using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{

    public float healthPoints;
    public bool isAlive = true;
    Rigidbody rb;
    bool isGoingToDie = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        Dead();

        if (healthPoints <= 0)
        {
            isAlive = false;
        }



    }

    private void OnCollisionEnter(Collision collision)
    {
       
        if (isGoingToDie == true)
        {
            isAlive = false;
        }
       
    }


    void Dead()
    {
        if (isAlive == false)
        {

            gameObject.SetActive(false);

        }
        if (rb.velocity.y <= -25)
        {
            isGoingToDie = true;
        }

    }
}

