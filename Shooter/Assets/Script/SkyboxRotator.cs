using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxRotator : MonoBehaviour {
    public float speed = 0.01f;
    public bool _rotate;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (_rotate)
        {
            GetComponent<Renderer>().material.mainTextureOffset = new Vector2(Time.time * speed, 0.0f);
        }
    }
}
