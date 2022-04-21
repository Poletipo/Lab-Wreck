using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieEnemy : MonoBehaviour {

    Transform player;
    NavMeshAgent agent;
    Health health;
    Hitable hitable;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Firearm>().transform;
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
        health.OnDeath += OnDeath;

        hitable = GetComponent<Hitable>();

    }

    private void OnDeath()
    {
        Destroy(gameObject);
    }

    void OnHit()
    {
        Debug.Log("Ouch");
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.position);
    }
}
