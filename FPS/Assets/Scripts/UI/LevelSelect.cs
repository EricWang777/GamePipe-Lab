using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour {

    public string levelToLoad = "Level1";
    public Text score;
    public Dropdown fun;

	// Use this for initialization
	void Start () {
        //PlayerPrefs.SetInt(levelToLoad, 0);
        checkScore();
        checkFun();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onLoadLevel()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    public void SetScore(int _score)
    {
        score.gameObject.SetActive(true);
        score.text = "Your score is " + _score.ToString();
    }

    void checkScore()
    {
        if(PlayerPrefs.HasKey(levelToLoad))
        {
            SetScore(PlayerPrefs.GetInt(levelToLoad));
        }
    }

    void checkFun()
    {
        if (PlayerPrefs.HasKey(levelToLoad + "fun"))
        {
            fun.value = PlayerPrefs.GetInt(levelToLoad + "fun");
        }
    }

    void OnApplicationQuit()
    {
        RemoveKey();
    }

    public void RemoveKey()
    {
        PlayerPrefs.DeleteKey(levelToLoad);
        PlayerPrefs.DeleteKey(levelToLoad +"fun");
    }

    public void OnValueChange(int value)
    {
        //Debug.Log(fun.value);
        PlayerPrefs.SetInt(levelToLoad + "fun", fun.value);
    }

    public bool hasFinished()
    {
        return PlayerPrefs.HasKey(levelToLoad);
    }

    public int GetFun()
    {
        return fun.value;
    }
}
