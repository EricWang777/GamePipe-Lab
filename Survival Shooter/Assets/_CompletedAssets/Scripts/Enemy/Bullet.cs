using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public int damage;
    
    CompleteProject.PlayerHealth playerHealth;                  // Reference to the player's health.
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public float GetDamage()
    {
        return damage;
    }

    public void SetDamage(int _damage)
    {
        damage = _damage;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            
            playerHealth = collision.gameObject.GetComponent<CompleteProject.PlayerHealth>();
            // If the player has health to lose...
            
            if (playerHealth.currentHealth > 0)
            {
                // ... damage the player.
                playerHealth.TakeDamage(damage);
            }
            
        }
    }
}
