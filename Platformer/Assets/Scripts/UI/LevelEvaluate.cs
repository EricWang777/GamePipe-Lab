using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class LevelEvaluate : MonoBehaviour {

    public LevelSelect[] levels;
    public InputField playerName;
    public GameObject warningText;
    public GameObject thankyou;

    private StreamWriter recordFile;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnSubmit()
    {
        if(playerName.text == "")
        {
            warningText.SetActive(true);
            return;
        }

        for(int i = 0; i < levels.Length; i++)
        {
            if(!levels[i].hasFinished())
            {
                warningText.SetActive(true);
                return;
            }
            else if (levels[i].GetFun() == 0)
            {
                warningText.SetActive(true);
                return;
            }
        }

        //Write all result to files
        string fileName = playerName.text + "_level_" + (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds + ".txt";
        new FileInfo(Application.dataPath + "/LevelEvaluation/" + fileName).Directory.Create();
        recordFile = new StreamWriter(Application.dataPath + "/LevelEvaluation/" + fileName, false);

        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i].hasFinished())
            {
                recordFile.WriteLine(levels[i].GetFun());
                levels[i].RemoveKey();
                levels[i].gameObject.SetActive(false);
            }
        }
        thankyou.SetActive(true);
        recordFile.Close();
    }
}
