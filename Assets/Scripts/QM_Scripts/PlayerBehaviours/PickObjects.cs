using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickObjects : MonoBehaviour {

    public bool isPickable = false;
    public bool isPicked = false;
    public GameObject hangingObjectPosition;
    [SerializeField]
    private GameObject otherGameObject;
    public GameObject cam;
    private float throwStrengthX;
    public float throwStrengh;
    private float throwStrengthY;
    public float throwHigh;

    
    private void Start()
    {
        otherGameObject = null;
        
    }

    private void Update()
    {
        StartCoroutine( PickUp(otherGameObject));
        StartCoroutine(Throw(otherGameObject));
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 13 && isPicked == false)
        {
            otherGameObject = other.gameObject;
            isPickable = true;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 13)
        {
            isPickable = false;

        }
    }


    private IEnumerator PickUp(GameObject other)
    {
        if (isPickable == true && isPicked == false)
        {
            if (Input.GetButtonUp("X"))
            {
                other.transform.parent = this.transform;
                other.transform.position = hangingObjectPosition.transform.position;
                other.GetComponent<Rigidbody>().isKinematic = true;
                other.GetComponent<Rigidbody>().detectCollisions = false;
                other.transform.rotation = this.transform.rotation;
                isPickable = false;
                yield return new WaitForEndOfFrame();
                isPicked = true;
            }
        }
    }

    private IEnumerator Throw(GameObject other)
    {
        if (isPicked == true && isPickable == false)
        {
            if (Input.GetButtonUp("X") || throwStrengthX >= 500)
            {
                other.transform.parent = null;
                other.GetComponent<Rigidbody>().isKinematic = false;
                isPicked = false;
                other.GetComponent<Rigidbody>().AddForce((transform.right * throwStrengthX) + (transform.up * throwStrengthY));
                throwStrengthX = 0;
                throwStrengthY = 0;
                yield return new WaitForEndOfFrame();
                //this.transform.GetComponent<CharacterController>().speed *= 3;
                
                yield return new WaitForSeconds(.01f);
                GetComponent<CharacterController>().speed *= 1.5f;
                other.GetComponent<Rigidbody>().detectCollisions = true;
                //cam.GetComponent<CameraController>().CameraDeZoomFocus();
            }

            if (Input.GetButton("X"))

            { 
                throwStrengthX += throwStrengh + Time.deltaTime;
                throwStrengthY += throwHigh + Time.deltaTime;
            }

            if (Input.GetButtonDown("X"))
            {
                GetComponent<CharacterController>().speed /= 1.5f;
                //cam.GetComponent<CameraController>().CameraZoomFocus(1);
                //this.transform.GetComponent<CharacterController>().speed /= 3;
                //this.transform.GetComponent<CharacterController>().speedRotationPlayer /= 3;


            }

            
        }
    }



}
