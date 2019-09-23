using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    //HP related
    public float maxHP = 100;
    protected float currentHP;
    public EnemyHP enemyHP;

    protected PlayerController pc;
    protected GameManager gm;

    protected bool isDead = false;

    //FX related
    public ParticleSystem deadParticle;

    // Use this for initialization
    void Start () {
        enemyHP.SetHP(1.0f);
        currentHP = maxHP;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Dead()
    {
        if(!isDead)
        {
            deadParticle.Play();
            Destroy(gameObject, 0.5f);
            SetVisible(false);
            gm.EnemyKilled();
            isDead = true;  
        }
        
    }

    public void TakeDamage(float damage)
    {
        currentHP = currentHP - damage;
        if (currentHP < 0)
        {
            currentHP = 0;
            Dead();
        }
        enemyHP.SetHP(currentHP / maxHP);
        
    }

    virtual protected void SetVisible(bool isVisible)
    {
        GetComponent<Renderer>().enabled = false;
        enemyHP.SetVisible(false);
    }
}
