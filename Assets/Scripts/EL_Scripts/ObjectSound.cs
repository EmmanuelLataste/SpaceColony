using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSound : MonoBehaviour {

    private IEnumerator coroutine;
    public LayerMask receiversMask;
    public float soundRadius;
    public float littleSoundRadius;
    public float bigSoundRadius;
    bool isTransmitting;



    public List<Transform> soundReceivers = new List<Transform>();
    public Collider[] soundTransmitter;
    [SerializeField] bool isObject;
    [SerializeField] bool isControlledEntity;
    CharacterController cc;

    private void Start() {
        //coroutine = SoundSource();
        if (isObject == false)
        cc = GetComponent<CharacterController>();
    }


    private void Update()
    {
        if (isObject == true)
            SoundTransmitterObj();

        else if (isControlledEntity == true)
            SoundTransmitterEnt();
    }

    void SoundTransmitterObj()
    {
        foreach(Collider collider in soundTransmitter)
        {
                if (collider.gameObject.GetComponent<FieldOfView>() != null && !collider.GetComponent<FieldOfView>().audibleTargets.Contains(transform))
                {
                collider.GetComponent<FieldOfView>().audibleTargets.Add(transform);
                collider.GetComponent<FieldOfView>().audible= true;
                    Debug.Log("TOUYIP");
                }


          
        }
        if (isTransmitting == true)
        {

            
            soundTransmitter = Physics.OverlapSphere(transform.position, soundRadius, receiversMask);

            isTransmitting = false;
        }

        else soundTransmitter = new Collider[0];
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (isTransmitting == false && isObject == true)
        {
            

            isTransmitting = true;
        }

    }

    void SoundTransmitterEnt()
    {
        foreach (Collider collider in soundTransmitter)
        {
            if (collider.gameObject != null && collider.gameObject.GetComponent<FieldOfView>() != null && !collider.GetComponent<FieldOfView>().audibleTargets.Contains(transform))
            {
                collider.GetComponent<FieldOfView>().audibleTargets.Add(transform);
                collider.GetComponent<FieldOfView>().audible = true;

            }
            
        }
        if (isTransmitting == true)
        {


            soundTransmitter = Physics.OverlapSphere(transform.position, soundRadius, receiversMask);

            isTransmitting = false;
        }

        else soundTransmitter = new Collider[0];

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            isTransmitting = true;

            if (cc.isCrouch == true)
            {
                soundRadius = littleSoundRadius;
            }

            else
            {
                soundRadius = bigSoundRadius;
            }
        }

        else
        {
            isTransmitting = false;
            soundRadius = 0;

        } 
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(transform.position, soundRadius);
    //}
    //private IEnumerator SoundSource() {
    //    gameObject.layer = 15;
    //    gameObject.tag = "SoundSource";
    //    yield return new WaitForSeconds(4f);
    //    gameObject.layer = 13;
    //    gameObject.tag = "Wood";
    //}

    //public void FindReceivers() {
    //    soundReceivers.Clear();
    //    Collider[] receiversInRadius = Physics.OverlapSphere(transform.position, soundRadius, receiversMask);

    //    if (receiversInRadius.Length > 0) {
    //        StartCoroutine(SoundSource());
    //    }
    //}
}
