using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {

    // delegate signature de fonction
    public delegate void MovementControllerEvent(MovementController mc);

    //Listeners
    public MovementControllerEvent OnDirectionChanged;
    public MovementControllerEvent OnJumping;
    public MovementControllerEvent OnFalling;
    public MovementControllerEvent OnLanding;
    public MovementControllerEvent OnCrouchingChange;


    private Vector2 _moveInput = Vector2.right;

    public Vector2 MoveInput {
        get { return _moveInput; }
        set {

            if (value.x != 0) {
                Vector3 oldDir = facingDirection;
                if (value.x > 0)
                {
                    facingDirection = Vector3.right;
                }
                else
                {
                    facingDirection = Vector3.left;
                }

                if(oldDir != facingDirection)
                {
                    OnDirectionChanged?.Invoke(this);
                }

            }
            _moveInput = value;
        }
    }


    private bool _isCrouched;

    public bool IsCrouched
    {
        get { return _isCrouched; }
        set { 
            _isCrouched = value;

            HandleCrouch();
        }
    }

    private void HandleCrouch()
    {
        float colliderSize = IsCrouched ? CrouchingSize : StandingSize;

        playerCollider.height = colliderSize;
        playerCollider.center = Vector3.up * (colliderSize / 2);
        OnCrouchingChange?.Invoke(this);
    }

    public float StandingSize = 2;
    public float CrouchingSize = 1;
    public CapsuleCollider playerCollider;

    public float Speed = 5;
    public float CrouchSpeed = 2;

    [Header("Jump Settings")]
    public float JumpHeight = 2;
    public float PreJumpBuffer = 0.1f;
    public float CoyoteTime = 0.1f;
    public float LowJumpMultiplier = 1.25f;
    public float FallMultiplier = 1.5f;
    public float MaxFallingSpeed = -20f;

    private float _preGroundedJumpTimer = 0;
    private float _coyoteTimer = 0;
    private float _isJumpingTimer = 0;

    private Vector2 _velocity;
    private bool _desiredJump;
    private float jumpVelocity;

    private bool _isInAir = false;
    private bool _isJumping = false;
    public bool _isGrounded = false;

    private bool _jumpInput;
    public bool JumpInput {
        get { return _jumpInput; }
        set {
            if (value && value != _jumpInput) {
                _desiredJump = true;
                _preGroundedJumpTimer = 0;
            }

            _jumpInput = value;
        }
    }

    public Vector2 Velocity { get { return _velocity; } }
    public float CurrentSpeed = 0;

    private int _layermask = ~(1 << 6);

    private Rigidbody rb;
    private Collider mcCollider;
    public Vector3 facingDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mcCollider = GetComponent<Collider>();
        jumpVelocity = Mathf.Sqrt(-2f * Physics2D.gravity.y * JumpHeight);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isJumping) {
            if (IsGrounded()) {
                _isGrounded = false;
            }
            else {
                _isJumping = false;
            }
        }
        else {
            _isGrounded = IsGrounded();
        }


        if (_isGrounded && _isInAir)
        {
            _isInAir = false;
            OnLanding?.Invoke(this);
        }

        _velocity = rb.velocity;

        UpdateTimers();
        UpdateJump();

        CurrentSpeed = IsCrouched ? CrouchSpeed : Speed;

        _velocity.x = CurrentSpeed * MoveInput.x;

        if (_desiredJump && (_isGrounded || _coyoteTimer <= CoyoteTime)) {
            Jump();
        }

        rb.velocity = _velocity;

        if (_velocity.y < 0 && !_isGrounded && !_isJumping) {
            OnFalling?.Invoke(this);
        }

    }

    private void FixedUpdate()
    {

    }

    private void UpdateJump()
    {
        if (_preGroundedJumpTimer > PreJumpBuffer) {
            _desiredJump = false;
        }
        if (_isJumpingTimer > 0.2f) {
            _isJumping = false;
        }

        if (_velocity.y < 10) {
            _velocity += Vector2.up * Physics2D.gravity * FallMultiplier * Time.deltaTime;
        }
        if (_velocity.y > 0 && !JumpInput) {
            _velocity += Vector2.up * Physics2D.gravity * LowJumpMultiplier * Time.deltaTime;
        }


        _velocity.y = Mathf.Clamp(_velocity.y, MaxFallingSpeed, 9999);
    }

    private void UpdateTimers()
    {
        _preGroundedJumpTimer += Time.deltaTime;
        _isJumpingTimer += Time.deltaTime;

        if (!_isGrounded) {
            _coyoteTimer += Time.deltaTime;
        }
        else {
            _coyoteTimer = 0;
        }
    }

    private void Jump()
    {
        _velocity.y = jumpVelocity;

        _isJumping = true;
        _isInAir = true;
        _isJumpingTimer = 0;

        _desiredJump = false;
        _coyoteTimer = CoyoteTime + 1;
        OnJumping?.Invoke(this);
    }

    private bool IsGrounded()
    {
        bool isGrounded = false;

        Vector3 playerFloor = new Vector3(mcCollider.bounds.center.x, mcCollider.bounds.center.y - mcCollider.bounds.extents.y, mcCollider.bounds.center.z);
        Vector3 groundedCollisionSize = new Vector3(mcCollider.bounds.extents.x * 2.0f * 0.9f, 0.3f, mcCollider.bounds.extents.z * 2.0f);

        Collider[] colliders = Physics.OverlapBox(playerFloor, groundedCollisionSize, Quaternion.identity, _layermask);
        DebugExtension.DrawBox(playerFloor, groundedCollisionSize, Color.red, 0.0f);

        if (colliders.Length > 0) {


            //Physics.Raycast(playerFloor,Vector3.down, _layermask,)




            isGrounded = true;
        }

        return isGrounded;
    }






}
