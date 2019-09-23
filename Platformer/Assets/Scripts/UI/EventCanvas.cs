using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventCanvas : MonoBehaviour {

    public Text eventText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetEventText(string _text)
    {
        eventText.text = _text;
    }
}
