using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour {

    public MovablePlatform[] platforms;

    private Vector3[] positions;
    private Quaternion[] rotations;

    // Use this for initialization
    void Start() {
        positions = new Vector3[platforms.Length];
        rotations = new Quaternion[platforms.Length];
        //Remember Initial positions
        for(int i = 0; i < platforms.Length; i++)
        {
            positions[i]= platforms[i].transform.position;
            rotations[i] = platforms[i].transform.rotation;
        }
    }

    // Update is called once per frame
    void Update() {

    }

    public void Triggered()
    {
        //Debug.Log(platforms.Length);
        for(int i = 0; i < platforms.Length; i++)
        {
            //reset to is initial position
            iTween.Stop(platforms[i].gameObject);
            platforms[i].gameObject.transform.position = positions[i];
            platforms[i].gameObject.transform.rotation = rotations[i];
            //Start the movement
            //
            platforms[i].EnableMove();
        }
    }
}
