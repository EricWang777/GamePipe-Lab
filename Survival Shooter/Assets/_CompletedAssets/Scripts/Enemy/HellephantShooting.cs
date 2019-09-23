using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompleteProject
{
    public class HellephantShooting : MonoBehaviour
    {
        // Bullet
        public GameObject bullet;
        //public Transform fireSocket;
        public float bulletSpeed = 2.0f;
        public float fireCooldown = -1.0f;
        private float fireTimer;
        int fireTimes = 0;


        // Use this for initialization
        void Start()
        {
            fireTimer = 0;
        }

        // Update is called once per frame
        void Update()
        {
            if (GetComponent<EnemyHealth>().currentHealth <= 0) return;
            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0 && fireCooldown > 0)
            {
                Fire();
                fireTimer += fireCooldown;
            }
        }

        private void Fire()
        {
            fireTimes++;

            for (int i = 0; i < 360; i += 25)
            {
                //Transform temp = transform;
                //temp.Rotate(0, i, 0);

                Quaternion bullet_rotation = Quaternion.Euler(0,i+fireTimes*10,0);
                GameObject projectile = Instantiate(bullet, transform.position, bullet_rotation);
                Destroy(projectile, 4.0f);
                

                projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * bulletSpeed;
            }

        }
    }

}
