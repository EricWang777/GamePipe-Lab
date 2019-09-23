using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.SceneManagement;

public class RandomEval : MonoBehaviour {

    public Dropdown fun;

    public GameObject warningText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnSubmit()
    {
        //Make sure player has indicated wheter the level or not
        if(fun.value == 0)
        {
            warningText.SetActive(true);
            return;
        }

        string folderName = PlayerPrefs.GetString("DataFolder");
        string fileName = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds + ".txt";

        //new FileInfo(Application.dataPath + "/random_data/" + folderName + fileName).Directory.Create();
        StreamWriter recordFile = new StreamWriter(Application.dataPath + "/random_data/" + folderName + "/" + fileName, false);

        //Write to data
        int totalEvents = PlayerPrefs.GetInt("EventsNum");
        for(int i = 0; i < totalEvents; i++)
        {
            recordFile.WriteLine(PlayerPrefs.GetInt("Event_" + i));
        }

        recordFile.WriteLine(PlayerPrefs.GetInt("BestEvent"));
        print(PlayerPrefs.GetInt("BestEvent"));

        if(fun.value == 1)
        {
            recordFile.WriteLine(1);
        }
        else if(fun.value == 2)
        {
            recordFile.WriteLine(0);
        }

        recordFile.Close();

        //Start a new random level
        SceneManager.LoadScene("Template");
    }
}
