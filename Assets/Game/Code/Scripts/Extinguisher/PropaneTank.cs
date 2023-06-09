using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropaneTank : MonoBehaviour {

    [SerializeField] Damageable _damageable;
    [SerializeField] GameObject _knob;
    [SerializeField] Transform _knobTransform;

    bool isActivated = false;


    // Start is called before the first frame update
    void Start()
    {
        _damageable.OnHit += OnHit;
    }

    private void OnHit(Damageable.HitData obj)
    {
        if (!isActivated) {
            isActivated = true;

            Instantiate(_knob, _knobTransform.position, _knobTransform.rotation);
            Destroy(_knobTransform.gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
