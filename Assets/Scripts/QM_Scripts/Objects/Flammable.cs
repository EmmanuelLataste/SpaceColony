using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flammable : MonoBehaviour

{
    [Header("Flammable")]
    public bool isBurning;
    public bool isStillBurning;
    [SerializeField] Transform firePosition;
    [SerializeField] float timerFire;
    [SerializeField] float radiusFire;
    [SerializeField] Collider[] fireArray;
    float timer;
    float initialTimer;
    GameObject particleFires;
    float life;
    bool canDamageFire;


    

    public void OnFire()
    {
        TurnFireOn();
        if (isBurning == true)
        {
            
           
            canDamageFire = true;
            
            ParticlesFire();
            
            timer = Time.time + timerFire;
            initialTimer = Time.time ;
            isStillBurning = true;
            isBurning = false;
        }

         if ( isStillBurning == true)
        {
            FireOnOthers();
            DamagesFire();
            WaitDamagesFire(1f);
            if (timer < Time.time)
            {
                StopFire();
            }
                
        }
    }

    public void StopFire()
    {
        
            isStillBurning = false;
            Destroy(particleFires);
        
    }

    void ParticlesFire()
    {
        particleFires = Instantiate(Resources.Load("ParticleFire"), firePosition.transform.position, Quaternion.identity) as GameObject;
        particleFires.transform.parent = this.transform;
        particleFires.transform.localScale = Vector3.one;
        Debug.Log("HELLO");

    }

    void DamagesFire()
    {
        if (GetComponent<Life>() != null)
        {
            if (timer > Time.time && canDamageFire == true)
            {
                GetComponent<Life>().Damages(.25f);
                canDamageFire = false;

            }
            
        }
       
    }

    void WaitDamagesFire(float timerBetweenDmg)
    {
       if (initialTimer <= Time.time)
        {
            initialTimer += timerBetweenDmg;
            canDamageFire = true;
        }

        else
        {
            canDamageFire = false;
            
        }
      
        
    }

    void FireOnOthers()
    {
        fireArray = Physics.OverlapSphere(firePosition.position, radiusFire);
        foreach (Collider collid in fireArray)
        {
            if (collid.gameObject.GetComponent<Flammable>() && collid.gameObject.GetComponent<Flammable>().isStillBurning == false )
            {
                if (collid.gameObject.layer == LayerMask.NameToLayer("Entity") && collid.gameObject.GetComponent<CharacterController>().gameObjectPicked == transform.gameObject)
                {
                    
                }


                else
                {
                  
                        collid.gameObject.GetComponent<Flammable>().isBurning = true;
                    
                    
                }
                
            }


            if (collid.gameObject.GetComponent<Explosive>())
            {
                StartCoroutine(collid.GetComponent<Explosive>().Boom());
                Debug.Log("NOTHING");
            }

        }
    }

    void TurnFireOn()
    {
        if (Input.GetKeyDown(KeyCode.R) && gameObject.tag == "Wood")
        {
            isBurning = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(firePosition.position, radiusFire);
    }
}
