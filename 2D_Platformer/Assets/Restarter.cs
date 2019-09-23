using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restarter : MonoBehaviour {
    public GameManager gm;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //deadsong.Play();
            //other.gameObject.SetActive(false);
            gm.ResetPlayer(false);

        }
    }
}
