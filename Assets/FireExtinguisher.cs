using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExtinguisher : MonoBehaviour {

    [SerializeField] Damageable damageable;
    [SerializeField] PressurePropulsion _pressurePropulsion;
    [SerializeField] Rigidbody _rb;


    // Start is called before the first frame update
    void Start()
    {

        damageable.OnHit += OnHit;

    }

    private void OnHit(Damageable.HitData data)
    {
        _rb.isKinematic = false;
        _rb.AddForceAtPosition(data.direction * data.force, data.position, ForceMode.Impulse);
        _pressurePropulsion.Activate();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
