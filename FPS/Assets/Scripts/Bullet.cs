using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private BulletType bulletType;
    private Transform target;
    private float flyingTime;
    private float speed;

    private bool fired = false;

    public ParticleSystem hitParticle;
    public ParticleSystem explosionParticle;

    public AnimationCurve curve;
    private float xScale;
    private float yScale;
    private float acTime; //accumulated time

    public GameObject bulletModel;
    public AudioSource explodeSound;

    private PlayerController pc;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (fired)
        {
            switch (bulletType)
            {
                case BulletType.Straight:
                    transform.Translate(0, 0, speed * Time.deltaTime, Space.Self);
                    break;
                case BulletType.Curve:
                    acTime = acTime + Time.deltaTime;
                    float offset = curve.Evaluate(acTime / flyingTime);
                    //Debug.Log(offset);
                    transform.Translate(0, 0, speed * Time.deltaTime, Space.Self);
                    bulletModel.transform.localPosition = new Vector3(offset * xScale, offset * yScale, 0);
                    break;
                default:
                    break;
            }
        }

    }

    public void FireBullet(PlayerController _pc, BulletType _bulletType, Transform _target, float _flyingTime = 2.5f, float _xScale = 0.0f, float _yScale = 0.0f)
    {
        transform.LookAt(_target);
        fired = true;
        //GetComponent<Renderer>().enabled = true;
        bulletType = _bulletType;
        target = _target;
        flyingTime = _flyingTime;
        speed = Vector3.Distance(target.position, transform.position) / _flyingTime;
        //Debug.Log(speed);
        xScale = _xScale;
        yScale = _yScale;
        pc = _pc;

        if(bulletType == BulletType.Curve)
        {
            acTime = 0;
        }

        //Destroy(gameObject, flyingTime + 0.5f);
    }

    public void TakeDamage(float damage)
    {
        if(damage > 0)
        {
            hitParticle.Play();
            fired = false;
            bulletModel.GetComponent<Renderer>().enabled = false;
            bulletModel.GetComponent<Collider>().enabled = false;
            Destroy(gameObject, 0.5f);
        }
    }

    public void Explode()
    {
        explosionParticle.Play();
        fired = false;
        bulletModel.GetComponent<Renderer>().enabled = false;
        bulletModel.GetComponent<Collider>().enabled = false;
        explodeSound.Play();
        pc.TakeDamage(1.0f);
        Destroy(gameObject, 0.5f);
    }
}
