using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePropulsion : MonoBehaviour {

    [SerializeField] Transform _exaust;
    [SerializeField] float _force = 5;

    Rigidbody _rb;


    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 forceToApply = _exaust.forward * _force * Time.deltaTime;
        _rb.AddForceAtPosition(forceToApply, _exaust.position, ForceMode.Force);
    }
}
