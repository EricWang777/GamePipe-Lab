using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour
{

    public bool customCursor = true;
    public Texture2D cursorTexture;

    // Use this for initialization
    void Start()
    {
        if (customCursor)
        {
            Cursor.SetCursor(cursorTexture, new Vector2(cursorTexture.width/2, cursorTexture.height/2), CursorMode.ForceSoftware);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
