using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace CompleteProject
{
    public class StartUI : MonoBehaviour
    {

        public GameObject playerNameCanvas;
        public InputField playerName;
        public GameObject playerNameEmpty;

        private EnemyManager em;
        // Use this for initialization
        void Start()
        {
            em = GameObject.FindObjectOfType<EnemyManager>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnStartClick()
        {
            playerNameCanvas.SetActive(false);
            em.setPlayerName(playerName.text);
            em.StartLevel();
        }
    }
}
   
