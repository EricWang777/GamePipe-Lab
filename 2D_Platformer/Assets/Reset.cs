using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour {

	// Use this for initialization
	void Start () {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetSceneAt(1).name);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
