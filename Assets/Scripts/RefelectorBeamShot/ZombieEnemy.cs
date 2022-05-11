using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class ZombieEnemy : MonoBehaviour {

    GameObject player;
    NavMeshAgent agent;
    Health health;
    Health playerHealth;
    Hitable hitable;

    public AudioClip[] punchSound;
    public AudioClip[] ExplosionSound;
    public GameObject StunnedStar;

    public GameObject Coin;
    public GameObject Explosion;

    public float MinHurtDistance = 5f;
    public float hitInterval = 5f;
    public float hitIntervalTimer = 0f;

    private bool _isStunned = false;
    private float _stunnedTimer = 1;




    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Instance.Player;
        playerHealth = player.GetComponent<Health>();
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
        health.OnHurt += OnHurt;
        health.OnDeath += OnDeath;

        hitable = GetComponent<Hitable>();
    }

    public void Setup(Vector3 position, Quaternion rotation, int hp)
    {
        health = GetComponent<Health>();
        health.MaxHp = hp;
        health.Hp = hp;

        transform.position = position;
        transform.rotation = rotation;
        gameObject.SetActive(true);
    }


    private void OnHurt()
    {
        AudioSource.PlayClipAtPoint(punchSound[UnityEngine.Random.Range(0, punchSound.Length)], transform.position, 1f);
    }

    private void OnDeath()
    {
        GameObject coin = PoolManager.GetPoolObject(Coin);
        coin.GetComponent<Coin>().Setup(transform.position, Quaternion.identity);


        //Instantiate(Explosion, transform.position, Quaternion.identity);
        GameObject explosion = PoolManager.GetPoolObject(Explosion);
        explosion.GetComponent<DestroyVFX>().Setup(transform.position, Quaternion.identity);



        //AudioSource.PlayClipAtPoint(ExplosionSound[UnityEngine.Random.Range(0, ExplosionSound.Length)], transform.position, 1f);
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }

    void OnHit()
    {
    }

    private void Update()
    {
        hitIntervalTimer += Time.deltaTime;

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < MinHurtDistance && hitIntervalTimer >= hitInterval && !_isStunned) {

            playerHealth.Hurt((int)(MinHurtDistance / distance));

            hitIntervalTimer = 0;
        }

        if (_isStunned) {
            _stunnedTimer += Time.deltaTime;
            if (_stunnedTimer >= 0.5f) {
                agent.isStopped = false;
                _isStunned = false;
                StunnedStar.SetActive(false);
            }
        }
    }


    public void Stun()
    {
        if (_isStunned) {
            return;
        }

        _isStunned = true;
        if (agent != null && agent.isOnNavMesh)
            agent.isStopped = true;
        StunnedStar.SetActive(true);
        _stunnedTimer = 0;

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        agent.SetDestination(player.transform.position);
    }
}
