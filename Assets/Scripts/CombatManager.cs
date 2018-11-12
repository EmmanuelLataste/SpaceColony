
using UnityEngine;

public class CombatManager : MonoBehaviour {

    public float speed;
	void Update ()
    {
        IA();
	}

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            other.gameObject.SetActive(false);
        }

        else if (other.gameObject.tag == ("Ennemy") && gameObject.GetComponent<EnnemiController>().enabled == true)
        {
            other.gameObject.SetActive(false);
        }

     
    }

    void IA()
    {
        transform.position = Vector3.MoveTowards(transform.position, GameObject.Find("Player").transform.position, speed * Time.deltaTime);
    }
}
