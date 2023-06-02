using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeForce : MonoBehaviour {

    [SerializeField] Vector3 _direction;
    [SerializeField] float _force = 1;
    [SerializeField] bool isLocal = true;

    Rigidbody _rb;



    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        if (isLocal) {
            _direction = transform.InverseTransformDirection(_direction);
        }

        _rb.AddForce(_direction * _force, ForceMode.VelocityChange);
        _rb.AddTorque(Random.onUnitSphere);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
