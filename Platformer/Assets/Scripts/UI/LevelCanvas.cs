using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelCanvas : MonoBehaviour {

    public Text levelText;
    public Button menuButton;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetLevelText(string _text)
    {
        levelText.text = _text;
    }

    public void ShowButton(bool _show)
    {
        menuButton.enabled = _show;
        menuButton.gameObject.SetActive(_show);
    }


    public void MainMenu()
    {
        if(!(GameObject.FindObjectOfType<GameManager>().isRandom))
        {
            SceneManager.LoadScene("LevelEvaluate");
        }
        else
        {
            SceneManager.LoadScene("REval");
        }
        
    }
}
