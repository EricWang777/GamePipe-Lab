using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace CompleteProject
{
    public class ScoreManager : MonoBehaviour
    {
        public static int lastScore;
        public static int score;        // The player's score.
        public static int currentEvent; // The current event number
        public static int enemyLeft;

        Text text;                      // Reference to the Text component.


        void Awake ()
        {
            // Set up the reference.
            text = GetComponent <Text> ();

            // Reset the score.
            lastScore = 0;
            score = 0;
            currentEvent = 0;
            enemyLeft = 0;
        }


        void Update ()
        {
            // Set the displayed text to be the word "Score" followed by the score value.
            text.text = "Score: " + score + "   Event: " + (currentEvent+1) + "     Enemy Left: " + enemyLeft;
        }

        public void SetText(string _text)
        {
            text.text = _text;
        }
    }
}