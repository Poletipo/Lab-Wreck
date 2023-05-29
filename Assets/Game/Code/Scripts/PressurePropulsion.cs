using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePropulsion : MonoBehaviour {

    enum PropulsionState {
        Sleep,
        Activated,
        Done
    }

    [SerializeField] ParticleSystem _vfxSmoke;
    [SerializeField] Transform _exaust;

    [SerializeField] AnimationCurve _propulsionCurve;

    [SerializeField] float _force = 5;
    [SerializeField] float _duration = 5;

    [Header("Collision")]
    [SerializeField] Collider _collider;

    Rigidbody _rb;

    float _activationStartTime;
    PropulsionState _currentState = PropulsionState.Sleep;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if (_currentState == PropulsionState.Activated) {

            float t = (Time.time - _activationStartTime) / _duration;

            float force = _propulsionCurve.Evaluate(t) * _force;
            _collider.material.dynamicFriction = Mathf.Clamp01(1f - _propulsionCurve.Evaluate(t));

            Vector3 forceToApply = -_exaust.forward * force * Time.deltaTime;
            _rb.AddForceAtPosition(forceToApply, _exaust.position, ForceMode.Acceleration);

            if (_activationStartTime + _duration < Time.time) {
                Stop();
            }
        }

    }

    void Stop()
    {
        _currentState = PropulsionState.Done;
        _vfxSmoke.Stop();
    }


    public void Activate()
    {
        if (_currentState == PropulsionState.Sleep) {
            _currentState = PropulsionState.Activated;
            _activationStartTime = Time.time;
            _vfxSmoke.Play();
        }
    }


}
