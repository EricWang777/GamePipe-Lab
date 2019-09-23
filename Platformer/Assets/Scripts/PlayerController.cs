using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public GameObject model;

    //Move related
    public float moveSpeed = 10;
    private bool canMove = false;
    public GameObject sphere;

    //Jump related
    public float jumpForce;
    private bool inAir = false;
    public bool enableRotate = true;

    //HP related
    public int maxHP = 3;
    private int currentHP;
    public BeatingHealthBar hpBar;

    //Damage related
    public GameObject hitParticle;

    //
    public AudioSource deadSound;

    private bool isChallenge = false;

    private GameManager gm;

    // Use this for initialization
    void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();

        if (isChallenge | gm.isRandom)
        {
            canMove = false;
        }
        else
        {
            canMove = true;
        }

        SetMaxHP(maxHP);
        SetHP(maxHP);

    }

    public void jump()
    {
        model.GetComponent<Rigidbody>().AddForce(new Vector3(0, jumpForce, 0));
        if (enableRotate)
        {
            iTween.RotateBy(model, iTween.Hash("name", "modelRotate", "z", -0.50, "easeType", "easeInOutCubic", "time", 0.5));
        }
        inAir = true;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (canMove)
        {
            transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
        }


        if (Input.GetAxis("Jump") > 0 && !inAir)
        {
            //Debug.Log(Time.time);
            //iTween.MoveAdd(model, iTween.Hash("y", -5, "eazeType", "linear", "time", 1.0,"delay", 1.0));
            //transform.localRotation = Quaternion.Euler(Vector3.zero);
            model.GetComponent<Rigidbody>().AddForce(new Vector3(0, jumpForce, 0));
            if (enableRotate)
            {
                iTween.RotateBy(model, iTween.Hash("name", "modelRotate", "z", -0.50, "easeType", "easeInOutCubic", "time", 0.5));
            }
            inAir = true;
        }

    }

    public void SetInAir(bool isInAir)
    {
        inAir = isInAir;
        if (!isInAir)
        {
            iTween.StopByName("modelRotate");
            model.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }

    public void TakeDamage(int damage = 1)
    {
        SetHP(currentHP - damage);
        iTween.StopByName("modelRotate");
        if (isChallenge)
        {
            //Restart the event
            if (hitParticle)
            {
                hitParticle.GetComponent<ParticleSystem>().Play();
            }
            Dead();
            gm.RestartEvent(currentHP);
        }
        else
        {
            if (hitParticle)
            {
                hitParticle.GetComponent<ParticleSystem>().Play();
            }
            Dead();
            gm.RestartEvent(currentHP);
        }
        //Restart the event
    }

    private void Dead()
    {
        canMove = false;
        model.SetActive(false);
        deadSound.Play();
    }


    void SetHP(int _currentHP)
    {
        currentHP = _currentHP;
        hpBar.currentValue = Mathf.Clamp(_currentHP - 0.1f, 0, maxHP);
        //Debug.Log(currentHP);

        

    }

    public void ResetHP()
    {
        SetHP(maxHP);
    }

    void SetMaxHP(int _maxHP)
    {
        maxHP = _maxHP;
        hpBar.maxValue = _maxHP;
        hpBar.MaxIcons = _maxHP;
    }

    public void EnableMove(bool _canMove)
    {
        canMove = _canMove;
    }


    public void SetChallengeMode(bool _isChallenge)
    {
        isChallenge = _isChallenge;
    }

    public void ResetPlayer()
    {
        model.SetActive(true);
        iTween.StopByName("modelRotate");
        model.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public int GetCurrentHP()
    {
        return currentHP;
    }
}
