using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType { Straight, Curve };
public class EnemyControllerType2 : EnemyController {

    public Transform firingPosition;

    //Time related
    public bool enableFiring = true;
    public float firingInterval = 2.0f;
    private float countdown;

    //Bullet related
    public GameObject Bullet;
    //public enum BulletType {Straight, Curve };
    public BulletType bulletType;

    public float flyingTime = 5.0f;
    public float xScale = 1.0f;
    public float yScale = 1.0f;


	// Use this for initialization
	void Start () {
        enemyHP.SetHP(1.0f);
        currentHP = maxHP;
        countdown = 0;

        pc = GameObject.FindObjectOfType<GameManager>().pc;
        gm = GameObject.FindObjectOfType<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
        if(enableFiring)
        {
            countdown = countdown - Time.deltaTime;
            if(countdown <= 0)
            {
                //reset timer and fire
                countdown = firingInterval;
                Fire();
            }
        }
		
	}

    private void Fire()
    {
        GameObject newBullet = Instantiate(Bullet, firingPosition.position, firingPosition.rotation, firingPosition);
        //Debug.Log(pc.transform.position);
        //newBullet.GetComponent<Bullet>().FireBullet(BulletType.Straight, pc.transform, 2.5f);
        newBullet.GetComponent<Bullet>().FireBullet(pc, bulletType, pc.transform, flyingTime, xScale, yScale);
    }

    private void Dead()
    {
        if(!isDead)
        {
            enableFiring = false;
            deadParticle.Play();
            Destroy(gameObject, 0.5f);
            SetVisible(false);
            isDead = true;
        }
        
    }
}
