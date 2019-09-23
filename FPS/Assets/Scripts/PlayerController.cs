using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //Pistol Damage
    public float pistolDamage = 40;

    //Pistol FX-related
    public GameObject pistolPivot;
    public GameObject pistolMuzzle;
    public GameObject pistolMuzzleSocket;
    public AudioSource pistolSound;

    private GameObject currentEnemy;

    //HP related
    public int maxHP;
    private float currentHP;
    public BeatingHealthBar hpBar;

    private bool isChallenge = false;

    private GameManager gm;

	// Use this for initialization
	void Start () {
        SetMaxHP(maxHP);
        SetHP(maxHP);

        gm = GameObject.FindObjectOfType<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {

        //Orient gun to points to cursor
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            pistolPivot.transform.LookAt(hit.point);
            //Current cursor points to a enemy
            
            if (hit.transform.tag == "Enemy")
            {
                //Debug.Log("Hit Enemy");
                currentEnemy = hit.transform.gameObject;
            }
            else
            {
                currentEnemy = null;
            }
        }

        //Pistol fires
        if(Input.GetButtonDown("Fire1"))
        {
            GameObject newMuzzle = Instantiate(pistolMuzzle, pistolMuzzleSocket.transform.position, pistolMuzzleSocket.transform.rotation, pistolMuzzleSocket.transform);
            Destroy(newMuzzle, 0.2f);
            pistolSound.Play();
            //If hit a enemy
            //Debug.Log(currentEnemy);
            if (currentEnemy)
            {
                if(currentEnemy.GetComponent<EnemyController>())
                {
                    currentEnemy.GetComponent<EnemyController>().TakeDamage(pistolDamage);
                }
                else
                {
                    currentEnemy.transform.parent.GetComponent<Bullet>().TakeDamage(pistolDamage);
                }
            }
        }
    }

    public void TakeDamage(float damage)
    {
        SetHP(currentHP - damage);
        gm.Penalty();
    }

    void SetMaxHP(int _maxHP)
    {
        maxHP = _maxHP;
        hpBar.maxValue = _maxHP;
        hpBar.MaxIcons = _maxHP;
    }

    void SetHP(float _currentHP)
    {
        currentHP = _currentHP;
        hpBar.currentValue = Mathf.Clamp(_currentHP - 0.01f, 0, maxHP);

        if(currentHP <= 0)
        {
            if(isChallenge)
            {
                gm.EventOver((int)currentHP);
            }
            else
            {
                gm.EventOver((int)currentHP);
            }
        }
    }

    public void SetChallenge(bool _isChallenge)
    {
        isChallenge = _isChallenge;
    }

    public int GetCurrentHP()
    {
        return (int)currentHP;
    }

    public void ResetHP()
    {
        SetHP(maxHP);
    }

}
