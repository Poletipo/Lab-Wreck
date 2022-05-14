using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {


    private float _trauma;

    public float Trauma {
        get { return _trauma; }
        set {
            value = Mathf.Clamp01(value);
            _trauma = value;
        }
    }


    private float _shakeIntensity = 0;

    public int TraumaPower = 2;

    public float MaxShakeIntensity = 1f;
    public Vector3 PositionMultiplier = Vector3.one;
    public float MaxShakeAngle = 45f;
    public float ShakeFrequency = 1f;

    private Vector3 _shakeOffset;
    private Vector3 _rotationOffset;

    // Start is called before the first frame update
    void Start()
    {
        _shakeOffset = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (Trauma > 0) {
            Trauma -= Time.deltaTime;
        }

    }


    public Vector3 GetPositionOffset()
    {
        float timer = Time.time * ShakeFrequency;


        _shakeIntensity = Mathf.Pow(Trauma, TraumaPower);

        _shakeOffset.x = GetPerlinNoise(151, timer) * _shakeIntensity * MaxShakeIntensity * PositionMultiplier.x;
        _shakeOffset.y = GetPerlinNoise(954, timer) * _shakeIntensity * MaxShakeIntensity * PositionMultiplier.y;
        _shakeOffset.z = GetPerlinNoise(598, timer) * _shakeIntensity * MaxShakeIntensity * PositionMultiplier.z;

        return _shakeOffset;
    }


    public Vector3 GetRotationOffset()
    {
        float timer = Time.time * ShakeFrequency;
        _rotationOffset.x = GetPerlinNoise(151, timer) * _shakeIntensity * MaxShakeAngle;
        _rotationOffset.y = GetPerlinNoise(954, timer) * _shakeIntensity * MaxShakeAngle;
        _rotationOffset.z = GetPerlinNoise(598, timer) * _shakeIntensity * MaxShakeAngle;


        return _rotationOffset;
    }

    private float GetPerlinNoise(int seed, float timer)
    {
        return (-0.5f + Mathf.PerlinNoise(timer + seed, timer + seed));
    }


    public void AddTrauma(float traumaValue)
    {
        Trauma += traumaValue;
    }

    public void SetTrauma(float traumaValue)
    {
        Trauma = traumaValue;
    }

    public void SetHigherTrauma(float traumaValue)
    {
        if (Trauma < traumaValue) {
            Trauma = traumaValue;
        }
    }

}
