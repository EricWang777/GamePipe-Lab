using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidModel : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject);
        if (collision.gameObject.layer == 8) // Layer 8 is for bullet
        {
            float damage = collision.gameObject.GetComponent<Bullet>().GetDamage();
            transform.parent.GetComponent<Asteroid>().TakeDamage(damage);
        }

        if (collision.gameObject.tag == "Player") // Hit player
        {
            //If hit the player, destory the asteroid and deal damage to player
            transform.parent.GetComponent<Asteroid>().TakeDamage(int.MaxValue);
            collision.gameObject.GetComponent<JetController>().TakeDamage(1.0f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8) // Layer 8 is for bullet
        {
            float damage = other.gameObject.GetComponent<Bullet>().GetDamage();
            transform.parent.GetComponent<Asteroid>().TakeDamage(damage);
        }

        if (other.gameObject.tag == "Player") // Hit player
        {
            //If hit the player, destory the asteroid and deal damage to player
            transform.parent.GetComponent<Asteroid>().TakeDamage(int.MaxValue);
            other.gameObject.GetComponent<JetController>().TakeDamage(1.0f);
        }
    }
}
