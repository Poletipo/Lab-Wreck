using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroundEnemy : MonoBehaviour
{

    NavMeshAgent navMeshAgent;
    Transform player;
    Health health;


    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();

        health.OnDeath += OnDeath;

        player = FindObjectOfType<Player>().transform;
    }

    private void OnDeath()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        navMeshAgent.SetDestination(player.position);
    }


    private void OnCollisionEnter(Collision collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if(player != null)
        {
            player.DowngradeDice();
            Destroy(gameObject);
        }

    }


}
