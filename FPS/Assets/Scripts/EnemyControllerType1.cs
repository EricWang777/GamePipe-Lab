using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerType1 : EnemyController {

   

    //Explode FX
    public ParticleSystem explodeParticle;
    public AudioSource explodeSound;

    //Ring-shaped timer, type1 specific
    public RingTimer rt;

    //Attack Damage
    public float explosionDamage = 1.0f;



    // Use this for initialization
    void Start () {
        enemyHP.SetHP(1.0f);
        currentHP = maxHP;
        if(rt)
        {
            rt.SetTimer(true, this);
        }

        pc = GameObject.FindObjectOfType<GameManager>().pc;
        gm = GameObject.FindObjectOfType<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
        //iTween.RotateBy(gameObject, iTween.Hash("name", "modelRotate", "y", -0.50, "easeType", "easeInOutCubic", "time", 2));
    }


    public void Attack()
    {
        if(!isDead)
        {
            //GameObject.FindObjectOfType<GameManager>();
            explodeSound.Play();
            explodeParticle.Play();
            SetVisible(false);
            Destroy(gameObject, 1.0f);
            pc.TakeDamage(explosionDamage);

            gm.EnemyKilled();
            isDead = true;
        }
        

    }

    override protected void SetVisible(bool isVisible)
    {
        GetComponent<Renderer>().enabled = false;
        if (rt)
        {
            rt.SetVisible(false);
        }
        enemyHP.SetVisible(false);
    }
}
