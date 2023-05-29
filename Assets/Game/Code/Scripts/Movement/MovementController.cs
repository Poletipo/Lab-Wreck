using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {

    [HideInInspector]
    public Vector2 MoveInput;

    private bool _jumpInput;

    public bool JumpInput {
        get { return _jumpInput; }
        set {
            if (value != _jumpInput) {
                if (value == true && jumpCount < MaxJumpCount) {
                    desiredJump = true;
                }
                _jumpInput = value;
            }

        }
    }

    [SerializeField] Rigidbody rb;

    [Header("Movement")]
    public Transform PlayerInputSpace = default;
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
    private Vector3 groundNormal;
    private float maxDotGroundAngle = 0;
    //movement
    private Vector3 Velocity;
    private Vector3 desiredVelocity;
    private Vector3 direction;
    private float acceleration = 0;
    private Vector3 finalDirection;

    //Jumping
    private bool desiredJump = false;
    private int jumpCount = 0;

    private bool _onGround;

    public bool OnGround {
        get { return _onGround; }
        set {
            if (value != _onGround) {
                if (value == true) {
                    jumpCount = 0;
                }
                _onGround = value;
                //GroundContactChanging();
            }
        }
    }

    private void GroundContactChanging()
    {
        if (OnGround) {
            rb.useGravity = false;
        }
        else {
            rb.useGravity = true;
        }
    }



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
        Velocity = rb.velocity;

        if (desiredJump) {
            Jump();
        }
        Move();

        rb.velocity = Velocity;
        ClearState();
    }

    private void Jump()
    {
        if (OnGround || jumpCount < MaxJumpCount) {
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
        if (!OnGround) {
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
        //TODO: Move player along ground surface
        direction = new Vector3(MoveInput.x, 0, MoveInput.y).normalized;
        direction = PlayerInputSpace.TransformDirection(direction);
        direction = direction - groundNormal * Vector3.Dot(direction, groundNormal);

        direction *= Mathf.Clamp01(MoveInput.magnitude);

        if (direction.magnitude > 0) {
            finalDirection = direction;
        }

        desiredVelocity = direction * MaxSpeed;

        Debug.DrawRay(transform.position, finalDirection * 3, Color.red);
        Debug.DrawRay(transform.position, groundNormal * 3, Color.blue);

        if (OnGround) {
            acceleration = MoveInput.magnitude > 0 ? MaxAcceleration : MaxDecceleration;
        }
        else {
            acceleration = MaxAirAcceleration;
        }

        float maxAccelerationRate = acceleration * Time.deltaTime;
        float maxAccelerationRateX = maxAccelerationRate * Mathf.Abs(finalDirection.x);
        float maxAccelerationRateZ = maxAccelerationRate * Mathf.Abs(finalDirection.z);

        Velocity.x = Mathf.MoveTowards(Velocity.x, desiredVelocity.x, maxAccelerationRateX);
        Velocity.z = Mathf.MoveTowards(Velocity.z, desiredVelocity.z, maxAccelerationRateZ);
    }

    private void Setup()
    {
        maxDotGroundAngle = Mathf.Cos(Mathf.Deg2Rad * MaxGroundAngle);
    }

    private void ClearState()
    {
        groundNormal = Vector3.zero;
    }

    private void InputReception()
    {
        //MoveInput.x = Input.GetAxisRaw("Horizontal");
        //MoveInput.y = Input.GetAxisRaw("Vertical");
        //JumpInput = Input.GetButton("Jump");
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
        OnGround = false;
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

        OnGround = groundNormal == Vector3.zero ? false : true;

    }
}
