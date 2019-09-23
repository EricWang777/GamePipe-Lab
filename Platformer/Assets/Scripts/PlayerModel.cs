using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            transform.GetComponentInParent<PlayerController>().SetInAir(false);
        }
        if (collision.gameObject.tag == "Obstacle")
        {
            //Debug.Log("Damage");
            transform.GetComponentInParent<PlayerController>().TakeDamage();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Obstacle")
        {
            transform.GetComponentInParent<PlayerController>().TakeDamage();
        }
        if (other.gameObject.GetComponent<EventTrigger>())
        {
            other.gameObject.GetComponent<EventTrigger>().Triggered();
        }
    }
}
