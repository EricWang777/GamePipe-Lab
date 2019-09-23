using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour {

    public GameObject HPBar;
    public GameObject HPBackground;
    private float currentHPper;

    private float maxHPScale;
    

	// Use this for initialization
	void Start () {
        maxHPScale = HPBackground.transform.localScale.x;
    }
	
	// Update is called once per frame
	void Update () {
        updateHP();
        gameObject.transform.LookAt(Camera.main.transform);
    }

    public void SetHP(float HPper)
    {
        currentHPper = HPper;
    }

    private void updateHP()
    {
        float currentHPScale = maxHPScale * currentHPper;
        HPBar.transform.localScale = new Vector3(currentHPScale, HPBar.transform.localScale.y, HPBar.transform.localScale.z);
        HPBar.transform.localPosition = new Vector3( (maxHPScale - HPBar.transform.localScale.x)/ 2, HPBar.transform.localPosition.y, HPBar.transform.localPosition.z);
    }

    public void SetVisible(bool isVisble)
    {
        HPBar.GetComponent<Renderer>().enabled = isVisble;
        HPBackground.GetComponent<Renderer>().enabled = isVisble;
    }

}
