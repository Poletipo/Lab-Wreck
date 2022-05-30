using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform TargetOffset;

    public CameraFocus[] targetFocus;

    [Range(0, 1)]
    public float smoothSpeed = 0.1f;

    private CameraShake _cameraShake;
    // Position
    private Vector3 offset;
    private Vector3 _desiredPosition;
    private Vector3 _smoothPosition;

    private Vector3 _finalPosition;
    //Rotation
    private Vector3 _desiredRotation;
    private Vector3 _finalRotation;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - TargetOffset.position;
        _desiredPosition = TargetOffset.position + offset;
        _smoothPosition = _desiredPosition;
        _finalPosition = _desiredPosition;

        _desiredRotation = transform.rotation.eulerAngles;
        _cameraShake = GetComponent<CameraShake>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Vector3 focusTarget = Vector3.zero;

        for (int i = 0; i < targetFocus.Length; i++) {

            focusTarget += (targetFocus[i].position - TargetOffset.position) * targetFocus[i].priority;
        }

        focusTarget /= targetFocus.Length;

        focusTarget += TargetOffset.position;

        _desiredPosition = focusTarget + offset;

        _smoothPosition = (_smoothPosition * (1 - smoothSpeed)) + (_desiredPosition * smoothSpeed);

        _finalPosition = _smoothPosition + _cameraShake.GetPositionOffset();


        _finalRotation = _desiredRotation + _cameraShake.GetRotationOffset();
        //Assign
        transform.position = _finalPosition;
        transform.rotation = Quaternion.Euler(_finalRotation);

    }


}
