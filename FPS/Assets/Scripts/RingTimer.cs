using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingTimer : MonoBehaviour {

    public float shrinkTime = 2.0f;
    public float delay = 0.5f;
    public GameObject bg;
    public GameObject timer;

    private float maxScale;
    private float currentScale;
    private float shrinkSpeed;

    private bool enableTimer = false;
    private EnemyControllerType1 ec;

    public AudioSource tickTock;
    public float maxPitch = 3.0f;

	// Use this for initialization
	void Start () {
        maxScale = bg.transform.localScale.x;
        currentScale = timer.transform.localScale.x;
        shrinkSpeed = maxScale / shrinkTime;
	}
	
	// Update is called once per frame
	void Update () {
        //gameObject.transform.LookAt(Camera.main.transform);
        if (currentScale < maxScale && enableTimer)
        {
            currentScale = currentScale + shrinkSpeed * Time.deltaTime;
            timer.transform.localScale = new Vector3(currentScale, currentScale, 1);
            if(currentScale > maxScale)
            {
                tickTock.pitch = maxPitch;
                transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                Invoke("TimerEnds", delay);
            }
        }
	}

    public void SetTimer(bool _enableTimer, EnemyControllerType1 _ec)
    {
        enableTimer = _enableTimer;
        ec = _ec;
        if(enableTimer)
        {
            tickTock.Play();
        }
        else
        {
            tickTock.Stop();
        }
    }

    private void TimerEnds()
    {
        ec.Attack();
        enableTimer = false;
        tickTock.Stop();
    }

    public void SetVisible(bool isVisble)
    {
        bg.GetComponent<Renderer>().enabled = isVisble;
        timer.GetComponent<Renderer>().enabled = isVisble;
    }
}
