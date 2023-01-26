using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    public Transform HeadPosition;

    public GameObject Projectile;
    public GameObject Destruction;
    public float ShootInterval = .5f;
    float ShootIntervalTimer = 999f;

    public GameObject BurstProjectile;
    public int burstCount = 10;
    public float BurstShootInterval = .5f;
    float BurstShootIntervalTimer = 999f;



    Health health;
    bool IsDead = false;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
        health.OnDeath += OnDeath;
    }

    private void OnDeath()
    {
        IsDead = true;
        Instantiate(Destruction, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsDead)
        {
            ShootIntervalTimer += Time.deltaTime;

            if(ShootIntervalTimer >= ShootInterval)
            {
                GameObject temp = PoolManager.GetPoolObject(Projectile);
                temp.SetActive(true);
                temp.GetComponent<Projectile>().Setup(HeadPosition.position,player.transform.position - HeadPosition.position, 1);
                ShootIntervalTimer = 0;        
            }


            BurstShootIntervalTimer += Time.deltaTime;

            if (BurstShootIntervalTimer >= BurstShootInterval)
            {

                for (int i = 0; i < burstCount; i++)
                {
                    GameObject temp = PoolManager.GetPoolObject(BurstProjectile);
                    temp.SetActive(true);
                    temp.GetComponent<Projectile>().Setup(HeadPosition.position,UnityEngine.Random.onUnitSphere, 1);
                }
                    BurstShootIntervalTimer = 0;
            }
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        Player player = collision.collider.GetComponent<Player>();

        if(player!= null)
        {
            
        }


    }


}
