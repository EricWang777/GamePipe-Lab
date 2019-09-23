using UnityEngine;
using System.Collections;
using System;
using System.IO;

namespace CompleteProject
{
    public class EnemyMovement : MonoBehaviour
    {
        Transform player;               // Reference to the player's position.
        PlayerHealth playerHealth;      // Reference to the player's health.
        EnemyHealth enemyHealth;        // Reference to this enemy's health.
        UnityEngine.AI.NavMeshAgent nav;               // Reference to the nav mesh agent.
        public static float spawnTime = 3.0f;
        float timer;
        Renderer[] renderers;


        void Awake ()
        {
            // Set up the references.
            player = GameObject.FindGameObjectWithTag ("Player").transform;
            playerHealth = player.GetComponent <PlayerHealth> ();
            enemyHealth = GetComponent <EnemyHealth> ();
            nav = GetComponent <UnityEngine.AI.NavMeshAgent> ();
            renderers = gameObject.GetComponentsInChildren<Renderer>();

            
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].enabled = true;
                for (int j = 0; j < renderers[i].materials.Length; j++)
                {
                    Color matColor = renderers[i].materials[j].color;
                    matColor.a = 0.2f;
                    renderers[i].materials[j].color = matColor;
                }
            }

            Collider[] c = gameObject.GetComponentsInChildren<Collider>();
            for(int i = 0; i < c.Length; i++)
            {
                c[i].enabled = false;
            }
            if(gameObject.GetComponent<HellephantShooting>() != null)
            {
                gameObject.GetComponent<HellephantShooting>().enabled = false;
            }
            else if(gameObject.GetComponent<BearShooting>() != null)
            {
                gameObject.GetComponent<BearShooting>().enabled = false;
            }else if(gameObject.GetComponent<EnemyAttack>() != null)
            {
                gameObject.GetComponent<EnemyAttack>().enabled = false;
            }

            Invoke("activate", spawnTime);
        }


        void Update ()
        {  
            if(timer < spawnTime)
            {
                timer += Time.deltaTime;
                return;
            }
            // If the enemy and the player have health left...
            if (enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
            {
                // ... set the destination of the nav mesh agent to the player.
                nav.SetDestination (player.position);

                Vector3 targetPostition = new Vector3(player.position.x,
                                        this.transform.position.y,
                                        player.position.z);
                this.transform.LookAt(targetPostition);
            }
            // Otherwise...
            else
            {
                // ... disable the nav mesh agent.
                nav.enabled = false;
            }
            
            
        }
        void activate()
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                for (int j = 0; j < renderers[i].materials.Length; j++)
                {
                    Color matColor = renderers[i].materials[j].color;
                    matColor.a = 1f;
                    renderers[i].materials[j].color = matColor;
                }
            }

            Collider[] c = gameObject.GetComponentsInChildren<Collider>();
            for (int i = 0; i < c.Length; i++)
            {
                c[i].enabled = true;
            }
            if (gameObject.GetComponent<HellephantShooting>() != null)
            {
                gameObject.GetComponent<HellephantShooting>().enabled = true;
            }
            else if (gameObject.GetComponent<BearShooting>() != null)
            {
                gameObject.GetComponent<BearShooting>().enabled = true;
            }
            else if (gameObject.GetComponent<EnemyAttack>() != null)
            {
                gameObject.GetComponent<EnemyAttack>().enabled = true;
            }
        }
        

    }
}