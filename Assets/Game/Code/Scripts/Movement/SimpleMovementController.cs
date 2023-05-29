using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovementController : MonoBehaviour {

    [SerializeField] Rigidbody _rb;
    public Vector2 MoveInput;
    public float Speed = 5;
    public Transform forwardTransform;

    Vector3 _direction = new Vector3();


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _direction.x = MoveInput.x;
        _direction.z = MoveInput.y;

        _direction = forwardTransform.TransformDirection(_direction);

        _rb.velocity = _direction * Speed;
    }
}
