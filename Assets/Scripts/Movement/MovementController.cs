using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {

    private Vector2 MoveInput;

    private bool _jumpInput;

    public bool JumpInput {
        get { return _jumpInput; }
        set {
            if (value != _jumpInput) {
                if (value == true) {
                    desiredJump = true;
                }
                _jumpInput = value;
            }

        }
    }



    Rigidbody rb;

    [Header("Movement")]
    public float MaxSpeed = 8;
    public float MaxAcceleration = 2;
    public float MaxDecceleration = 2;
    public float MaxAirAcceleration = 2;
    public float MaxGroundAngle = 45;

    [Header("Jumping")]
    public float FallMultiplier = 2.5f;
    public float LowJumpMultiplier = 2f;
    public float JumpHeight = 2;
    public int MaxJumpCount = 2;

    // PRIVATE
    Vector3 groundNormal;
    float maxDotGroundAngle = 0;
    //movement
    Vector3 Velocity;
    Vector3 desiredVelocity;
    Vector3 direction;
    float acceleration = 0;

    //Jumping
    bool desiredJump = false;
    bool onGround = false;
    int jumpCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        InputReception();

        UpdateJump();
    }


    private void FixedUpdate()
    {
        ClearState();
        Velocity = rb.velocity;

        //TODO: fixed number of jump
        // If desired jump in air, jumps on next onGround
        if (desiredJump) {
            Jump();
        }
        Move();

        rb.velocity = Velocity;
    }

    private void Jump()
    {
        if (onGround || jumpCount < MaxJumpCount) {
            Debug.Log(onGround);
            //onGround = false;
            desiredJump = false;
            jumpCount++;
            float jumpVelocity = Mathf.Sqrt(-2f * Physics.gravity.y * JumpHeight);

            if (Velocity.y > 0f) {
                jumpVelocity = Mathf.Max(jumpVelocity - Velocity.y, 0f);
            }

            Velocity.y += jumpVelocity;
        }
    }


    private void UpdateJump()
    {
        if (!onGround) {
            //Better Jump
            if (rb.velocity.y < 0) {
                rb.velocity += Vector3.up * Physics.gravity.y * (FallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !JumpInput) {
                rb.velocity += Vector3.up * Physics.gravity.y * (LowJumpMultiplier - 1) * Time.deltaTime;
            }
        }
    }

    private void Move()
    {
        direction = new Vector3(MoveInput.x, 0, MoveInput.y);
        direction = Vector3.ClampMagnitude(direction, 1); ;

        desiredVelocity = direction * MaxSpeed;

        Debug.DrawRay(transform.position, direction * 3, Color.red);

        if (onGround) {
            acceleration = MoveInput.magnitude > 0 ? MaxAcceleration : MaxDecceleration;
        }
        else {
            acceleration = MaxAirAcceleration;
        }


        float maxAccelerationRate = acceleration * Time.deltaTime;

        Velocity.x = Mathf.MoveTowards(Velocity.x, desiredVelocity.x, maxAccelerationRate);
        Velocity.z = Mathf.MoveTowards(Velocity.z, desiredVelocity.z, maxAccelerationRate);
    }

    private void Setup()
    {
        MoveInput = new Vector2();
        rb = GetComponent<Rigidbody>();
        maxDotGroundAngle = Mathf.Cos(Mathf.Deg2Rad * MaxGroundAngle);
    }

    private void ClearState()
    {
        groundNormal = Vector3.zero;
        if (onGround) {
            jumpCount = 0;
        }
    }

    private void InputReception()
    {
        MoveInput.x = Input.GetAxisRaw("Horizontal");
        MoveInput.y = Input.GetAxisRaw("Vertical");
        JumpInput = Input.GetButton("Jump");
    }

    private void OnCollisionEnter(Collision collision)
    {
        EvaluationCollision(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        EvaluationCollision(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        onGround = false;
    }

    private void EvaluationCollision(Collision collision)
    {

        for (int i = 0; i < collision.contactCount; i++) {

            Vector3 contactNormal = collision.contacts[i].normal;
            float dot = Vector3.Dot(Vector3.up, contactNormal);

            if (dot >= maxDotGroundAngle) {
                groundNormal += contactNormal;
            }
        }

        groundNormal.Normalize();

        onGround = groundNormal == Vector3.zero ? false : true;

    }
}
