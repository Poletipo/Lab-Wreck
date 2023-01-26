using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public GameObject HitEffect;
    public IDamageable hitable;
    public LayerMask TargetingMask;
    public LayerMask BlockerMask;
    public float AggroDistance = 10;

    Health health;
    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
        health.OnHurt += OnHurt;
        health.OnDeath += OnDeath;
    }

    private void OnDeath()
    {
        Destroy(gameObject);
    }

    private void OnHurt()
    {
        Instantiate(HitEffect, hitable.HitPosition, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter(Collider other)
    {

        if(Utilities.IsInLayerMask(other.gameObject, TargetingMask))
        {
            Vector3 direction = other.bounds.center - transform.position;

            Ray ray = new Ray(transform.position, direction);

            RaycastHit hit;

            Physics.Raycast(ray, out hit, (AggroDistance + 1), BlockerMask);


            if(hit.collider == other)
            {
                Debug.Log($"{other.gameObject.name} in sight");
            }

        }


    }



}
