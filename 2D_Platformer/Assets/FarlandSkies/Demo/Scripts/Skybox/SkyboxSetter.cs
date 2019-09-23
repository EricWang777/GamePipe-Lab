using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxSetter : MonoBehaviour
{

    public Material skybox;

    // Use this for initialization
    void Start()
    {
        if (skybox)
        {
            RenderSettings.skybox = skybox;
            RenderSettings.skybox.SetFloat("_Rotation", 0);
        }


    }

}
