using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePlatform : MonoBehaviour {

    public int x = 3;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void EnableMove()
    {
        Invoke("iTweenMove", 0.1f);
    }

    void iTweenMove()
    {
        iTween.MoveBy(gameObject, iTween.Hash("x", x, "easeType", "linear", "loopType", "pingPong"));
    }
}
