using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger");
        if(other.transform.parent.GetComponent<Bullet>())
        {
            other.transform.parent.GetComponent<Bullet>().Explode();
        }
    }
}
