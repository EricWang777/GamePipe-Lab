using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private float damage;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public float GetDamage()
    {
        return damage;
    }

    public void SetDamage(float _damage)
    {
        damage = _damage;
    }
}
