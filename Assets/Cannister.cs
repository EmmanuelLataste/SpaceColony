using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannister : Explosive {
    ConstantForce cf;

    private void Start()
    {
        cf = GetComponent<ConstantForce>();   
    }

    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if (collision.gameObject.layer == LayerMask.NameToLayer("Entity"))
        {
            StartCoroutine(Fly());
        }

    }

    IEnumerator Fly()
    {
        Debug.Log("Chargine");
        yield return new WaitForSeconds(2);
        cf.relativeForce = Vector3.Lerp( new Vector3(0, 25.5f, 0), new Vector3(0,30,0), 3);
        yield return new WaitForSeconds(2);
        StartCoroutine(Boom());

        //rb.AddForce( new Vector3(0, 1500, 0), ForceMode.Acceleration);

    }
}
