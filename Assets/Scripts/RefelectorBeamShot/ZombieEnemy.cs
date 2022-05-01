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

    public GameObject StunnedStar;

    public GameObject Coin;

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
        health.OnDeath += OnDeath;

        hitable = GetComponent<Hitable>();

    }

    private void OnDeath()
    {
        Instantiate(Coin, transform.position, UnityEngine.Random.rotation);
        Destroy(gameObject);
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
