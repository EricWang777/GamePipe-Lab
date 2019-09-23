using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowinjg : MonoBehaviour {
    public Transform character;
    public float smoothTime = 0.01f;
    private Vector3 cameraVelocity = Vector3.zero;
    private Camera mainCamera;

    public Vector3 cameraOffset = new Vector3(5, 8, -15);

    void Awake()
    {
        mainCamera = Camera.main;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //transform.position = Vector3.SmoothDamp(transform.position, character.position + new Vector3(5, 8, -15), ref cameraVelocity, smoothTime);
        transform.position = character.position + cameraOffset;
    }
}
