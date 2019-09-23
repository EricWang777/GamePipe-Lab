using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {

    public float maxHealth = 100;
    private float currentHealth;
    public GameObject deadParticle;

    public GameObject model;

    public bool enableMove = false;
    public float moveSpeed = 0.5f;
    public float rotateSpeed = 20;

    public AudioSource explosionSound;

	// Use this for initialization
	void Start () {
        currentHealth = maxHealth;
    }
	
	// Update is called once per frame
	void Update () {
		if(enableMove)
        {
            //Move the asteroid down
            transform.Translate(0, 0, -moveSpeed * Time.deltaTime);
            model.transform.Rotate(rotateSpeed * Time.deltaTime, 0, 0, Space.Self);
        }
	}

    

    void Dead()
    {
        //Debug.Log("Dead");
        model.GetComponent<Renderer>().enabled = false;
        model.GetComponent<Collider>().enabled = false;
        deadParticle.SetActive(true);
        explosionSound.Play();
        GameObject.Destroy(gameObject, 1.0f);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Dead();
        }
    }
}
