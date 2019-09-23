using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class RandomUI : MonoBehaviour {

    public InputField playerName;
    public GameObject warningText;
    public int eventNum = 5;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnStartClicked()
    {
        if (playerName.text == "")
        {
            warningText.SetActive(true);
            return;
        }

        try
        {
            string folderName = playerName.text + "_random";
            Directory.CreateDirectory(Application.dataPath + "/random_data/" + folderName);
            PlayerPrefs.SetString("DataFolder", folderName);
        }
        catch (Exception e)
        {
            Debug.Log("Cannot create directory" + e.ToString());
            return;
        }


        //Start an random level
        PlayerPrefs.SetInt("EventsNum", eventNum);
        SceneManager.LoadScene("Template");
        
    }
}
