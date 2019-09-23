using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


namespace CompleteProject
{
    public class PlayerHealth : MonoBehaviour
    {
        public int startingHealth = 30;                            // The amount of health the player starts the game with.
        public int currentHealth;                                   // The current health the player has.
        //public Slider healthSlider;                                 // Reference to the UI's health bar.
        public Image damageImage;                                   // Reference to an image to flash on the screen on being hurt.
        public AudioClip deathClip;                                 // The audio clip to play when the player dies.
        public float flashSpeed = 5f;                               // The speed the damageImage will fade at.
        public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     // The colour the damageImage is set to, to flash.
        private EnemyManager enemyManager;
        public BeatingHealthBar hpBar;

        Animator anim;                                              // Reference to the Animator component.
        AudioSource playerAudio;                                    // Reference to the AudioSource component.
        AudioClip normalHurt;
        PlayerMovement playerMovement;                              // Reference to the player's movement.
        PlayerShooting playerShooting;                              // Reference to the PlayerShooting script.
        public bool isDead;                                                // Whether the player is dead.
        bool damaged;                                               // True when the player gets damaged.
        public float godTime = 1.0f;                                       // the time when player is invincible after take damage
        float timer;
        bool godlike;
        GameObject shield;

        void Awake ()
        {
            // Setting up the references.
            anim = GetComponent <Animator> ();
            playerAudio = GetComponent <AudioSource> ();
            playerMovement = GetComponent <PlayerMovement> ();
            playerShooting = GetComponentInChildren <PlayerShooting> ();
            enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
            shield = GameObject.FindGameObjectWithTag("Shield");
            // Set the initial health of the player.
            currentHealth = startingHealth;
            shield.SetActive(false);
            normalHurt = playerAudio.clip;
        }

        public void Reset()
        {
            anim = GetComponent<Animator>();
            playerAudio = GetComponent<AudioSource>();
            playerMovement = GetComponent<PlayerMovement>();
            playerShooting = GetComponentInChildren<PlayerShooting>();
            isDead = false;
            playerAudio.clip = normalHurt;
            
            
            // Set the initial health of the player.
            currentHealth = startingHealth;
            shield.SetActive(false);
            //healthSlider.value = startingHealth;
            hpBar.currentValue = hpBar.maxValue - 0.01f;
        }

        void Update ()
        {
            if (godlike)
            {
                timer -= Time.deltaTime;
            }
            if(timer <= 0 && godlike)
            {
                timer += godTime;
                godlike = false;
                
                shield.SetActive(false);
                
            }
            // If the player has just been damaged...
            if(damaged)
            {
                // ... set the colour of the damageImage to the flash colour.
                damageImage.color = flashColour;
            }
            // Otherwise...
            else
            {
                // ... transition the colour back to clear.
                damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
            }

            // Reset the damaged flag.
            damaged = false;
        }


        public void TakeDamage (int amount)
        {
            if (godlike) return;
            if (!godlike)
            {

                godlike = true;
                timer = godTime;
                
                shield.SetActive(true);
            }
            
            // Set the damaged flag so the screen will flash.
            damaged = true;

            // Reduce the current health by the damage amount.
            currentHealth -= amount;

            // Set the health bar's value to the current health.
            //healthSlider.value = currentHealth;
            hpBar.currentValue--;

            // Play the hurt sound effect.
            playerAudio.Play ();

            // If the player has lost all it's health and the death flag hasn't been set yet...
            if(currentHealth <= 0 && !isDead)
            {
                // ... it should die.
                
                Death ();
            }
        }


        void Death ()
        {
            // Set the death flag so this function won't be called again.
            isDead = true;

            // Turn off any remaining shooting effects.
            playerShooting.DisableEffects ();

            // Tell the animator that the player is dead.
            anim.SetTrigger ("Die");

            // Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
            playerAudio.clip = deathClip;
            playerAudio.Play ();

            // Turn off the movement and shooting scripts.
            playerMovement.enabled = false;
            playerShooting.enabled = false;

            enemyManager.EventOver();
            
        }


        //public void RestartLevel ()
        //{
        //    // Reload the level that is currently loaded.
        //    SceneManager.LoadScene (0);
        //}

        public void ResetHP()
        {
            currentHealth = startingHealth;
        }
    }
}