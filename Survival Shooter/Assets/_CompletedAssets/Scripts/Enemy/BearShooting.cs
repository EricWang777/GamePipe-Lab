using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompleteProject
{
    public class BearShooting : MonoBehaviour
    {
        // Bullet
        public GameObject bullet;
        //public Transform fireSocket;
        public float bulletSpeed = 2.0f;
        public float fireCooldown = -1.0f;
        private float fireTimer;
       

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

            Quaternion bullet_rotation = transform.rotation;
            float randomX = UnityEngine.Random.insideUnitCircle.x * 8;
            GameObject projectile = Instantiate(bullet, transform.position, bullet_rotation);
            Destroy(projectile, 4.0f);
            projectile.transform.Rotate(0, randomX, 0);
            projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * bulletSpeed;

            //for (int i = 0; i < 360; i += 60)
            //{
            //    Transform temp = transform;
            //    temp.Rotate(0, i, 0);

                

            //    projectile.GetComponent<Rigidbody>().velocity = temp.forward * bulletSpeed;
            //}

        }
    }

}
