using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour {
    public bool custom;
    public Texture2D cursorTexture;
    // Use this for initialization
    void Start () {
        if (custom)
        {
            
            Cursor.SetCursor(cursorTexture, new Vector2(cursorTexture.width / 2, cursorTexture.height / 2), CursorMode.ForceSoftware);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
