using UnityEngine;
using UnityEngine.UI;

namespace CompleteProject
{
    public class GameOverManager : MonoBehaviour
    {
        public PlayerHealth playerHealth;       // Reference to the player's health.


        public static Animator anim;                          // Reference to the animator component.
        public static bool inEvent = true;


        void Awake ()
        {
            // Set up the reference.
            anim = GetComponent <Animator> ();
            
        }


        void Update ()
        {
            // If the player has run out of health...
            if(playerHealth.currentHealth <= 0 && inEvent)
            {
                // ... tell the animator the game is over.
                //anim.SetTrigger ("GameOver");
                inEvent = false;
            }

            
        }

        public static void EventOver()
        {
            inEvent = true;
            anim.SetTrigger("New State");
        }
        
    }
}