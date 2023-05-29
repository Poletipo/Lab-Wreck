using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPVCamera : MonoBehaviour {

    public Vector2 AimInput;
    [SerializeField] Transform yawTransform;

    public Vector2 speedMultiplier = Vector2.one;




    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        yawTransform.Rotate(Vector3.up * AimInput.x * speedMultiplier.x);
        transform.Rotate(Vector3.left * AimInput.y * speedMultiplier.y);
    }
}
