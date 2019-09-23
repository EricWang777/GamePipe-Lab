using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ChallengeUI : MonoBehaviour {

    private GameManager gm;

    public GameObject playerNameCanvas;
    public InputField playerName;
    public GameObject playerNameEmpty;

	// Use this for initialization
	void Start () {
        gm = GameObject.FindObjectOfType<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnStartClicked()
    {
       if(playerName.text == "")
        {
            playerNameEmpty.SetActive(true);
        }
       else
        {
            //Hide UI
            playerNameCanvas.SetActive(false);
            //Start Game
            gm.StartGame(playerName.text + "_" + (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
            //Start data collection
        }
    }
}
