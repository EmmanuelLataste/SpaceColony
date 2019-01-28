using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannister : Explosive {
    ConstantForce cf;
    public bool isFlying;
    [SerializeField] float timerBoom;
    [SerializeField] float timerFly;

    private void Start()
    {
        cf = GetComponent<ConstantForce>();   
    }

    private void Update()
    {
        if (isFlying == true)
        {
            StartCoroutine(Fly());
           
        }
    }

    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if (collision.gameObject.layer != LayerMask.NameToLayer("Obstacles") && isFlying == false)
        {
            isFlying = true;
            
        }

    }

    public IEnumerator Fly()
    {

        yield return new WaitForSeconds(timerFly);
        cf.relativeForce = Vector3.Lerp( new Vector3(0, 25.5f, 0), new Vector3(0,30,0), 3);
        yield return new WaitForSeconds(timerBoom);
        StartCoroutine(Boom());
        yield return null;

        //rb.AddForce( new Vector3(0, 1500, 0), ForceMode.Acceleration);

    }

 
}
