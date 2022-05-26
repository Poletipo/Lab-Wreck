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
    public GameObject BurnMark;

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
        playerHealth.OnDeath += OnPlayerDeath;

        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
        health.OnHurt += OnHurt;
        health.OnDeath += OnDeath;

        hitable = GetComponent<Hitable>();
    }

    private void OnPlayerDeath()
    {
        //agent.isStopped = true;
    }

    public void Setup(Vector3 position, Quaternion rotation, int hp)
    {
        health = GetComponent<Health>();
        health.MaxHp = hp;
        health.Hp = hp;

        transform.position = position;
        transform.rotation = rotation;
        StopStun();

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

        StencilSpawner.SpawnStencil(BurnMark, 1.5f, new Vector3(transform.position.x, 0.1f, transform.position.z), Quaternion.Euler(90, 0, UnityEngine.Random.Range(0f, 360f)));
        GameObject explosion = PoolManager.GetPoolObject(Explosion);
        explosion.GetComponent<DestroyVFX>().Setup(transform.position, Quaternion.identity);


        float shakeValue = (1.0f - (Vector3.Distance(transform.position, player.transform.position) / 15f));
        GameManager.Instance.CameraObject.GetComponent<CameraShake>().AddTrauma(shakeValue);
        AudioSource.PlayClipAtPoint(ExplosionSound[UnityEngine.Random.Range(0, ExplosionSound.Length)], transform.position, 1f);
        StopStun();

        gameObject.SetActive(false);
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
                StopStun();
            }
        }
    }


    public void Stun()
    {
        if (_isStunned) {
            return;
        }

        _isStunned = true;
        if (agent != null && agent.isOnNavMesh) {
            agent.updateRotation = false;
            agent.isStopped = true;
        }
        StunnedStar.SetActive(true);
        _stunnedTimer = 0;

    }

    public void StopStun()
    {
        if (agent != null && agent.isOnNavMesh) {
            agent.isStopped = false;
        }
        if (agent != null)
            agent.updateRotation = true;
        _isStunned = false;
        StunnedStar.SetActive(false);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        agent.SetDestination(player.transform.position);
    }
}
