using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EventCanvas : MonoBehaviour {

    public Text eventText;
    public Button mainMenu;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetText(string _text)
    {
        eventText.text = _text;
    }

    public void ShowButton(bool _show)
    {
        mainMenu.gameObject.SetActive(_show);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("LevelEvaluate");
    }
}
