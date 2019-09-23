using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JetController : MonoBehaviour {

    public float speed = 1.0f;
    public Vector2 boundingBox;
    public float roll = 20.0f;

    //Bullet to fire
    public GameObject bullet;
    public float fireCooldown = 0.4f;
    public Transform fireSocket;
    public float bulletSpeed = 5.0f;
    private float fireTimer;

    //HP related
    public float maxHP;
    private float currentHP;
    public BeatingHealthBar hpBar;
    public Image redFlash;
    public int redFlashTimes = 5;
    public float redFlashInterval = 0.5f;
    private int redFlashCounter;

    // Use this for initialization
    void Start () {
        SetMaxHP();
        SetCurrentHP(maxHP);
        fireTimer = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Handle jet movement
        Vector3 newPosition = transform.position;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        if (Input.GetAxis("Horizontal") > 0)
        {
            newPosition.x = newPosition.x + speed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, 0, -roll);
        }
        else if(Input.GetAxis("Horizontal") < 0)
        {
            newPosition.x = newPosition.x - speed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, 0, roll);
        }

        if (Input.GetAxis("Vertical") > 0)
        {
            newPosition.z = newPosition.z + speed * Time.deltaTime;
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            newPosition.z = newPosition.z - speed * Time.deltaTime;
        }
        newPosition.x = Mathf.Clamp(newPosition.x, -boundingBox.x, boundingBox.x);
        newPosition.z = Mathf.Clamp(newPosition.z, -boundingBox.y, boundingBox.y);
        transform.position = newPosition;

        //Handle jet firing
        if (Input.GetButton("Fire1"))
        {
            fireTimer -= Time.deltaTime;
            if(fireTimer <= 0)
            {
                Fire();
                fireTimer += fireCooldown;
            }
        }
        else
        {
            fireTimer = 0;
        }
	}

    private void Fire()
    {
        GameObject projectile = Instantiate(bullet, fireSocket.position, fireSocket.rotation);
        Destroy(projectile, 3.0f);
        //Set speed for the bullet
        projectile.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, bulletSpeed);
        //Set damage for the bullet
        projectile.GetComponent<Bullet>().SetDamage(100);
    }

    public void TakeDamage(float damage)
    {
        SetCurrentHP(currentHP - damage);
        //Red flash
        redFlashCounter = redFlashTimes;
        Invoke("RedFlashOn", 0.0f);
        if (currentHP <= 0)
        {
            Dead();
        }
    }

    void RedFlashOn()
    {
        redFlash.color = new Color(1.0f, 0, 0, 0.3f);
        Invoke("RedFlashOff", redFlashInterval);
    }

    void RedFlashOff()
    {
        redFlash.color = new Color(1.0f, 0, 0, 0.0f);
        redFlashCounter--;
        if(redFlashCounter > 0)
        {
            Invoke("RedFlashOn", redFlashInterval);
        }
    }

    private void Dead()
    {

    }


    void SetMaxHP()
    {
        hpBar.MaxIcons = (int)(Mathf.Ceil(maxHP));
        hpBar.maxValue = (int)(Mathf.Ceil(maxHP));
    }
    void SetCurrentHP(float _currentHP)
    {
        currentHP = _currentHP;
        hpBar.currentValue = Mathf.Clamp(currentHP - 0.1f, 0, maxHP);
    }
}
