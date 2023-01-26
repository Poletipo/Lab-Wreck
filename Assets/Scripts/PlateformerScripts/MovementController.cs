using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovementController : MonoBehaviour {

    // PUBLIC VARIABLE
    public Transform playerInputSpace = default;
    [Header("------------------------------")]

    #region MovementVariables
    [Header("Movement")]
    [Range(0, 50)]
    [SerializeField]private float _maxSpeed = 5;
    [Range(0, 50)]
    [SerializeField] private float _maxAcceleration = 1;
    [Range(0, 50)]
    [SerializeField] private float _maxAirAcceleration = 0.5f;
    [Range(0, 50)]
    [SerializeField] private float _maxDecceleration = 1;
    [Range(0, 90)]
    [SerializeField] private float _maxFloorAngle = 35;

    #endregion MovementVariables

    #region JumpVariables
    [Header("Jump")]
    [Range(0, 10)]
    public float JumpHeight = 2;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    #endregion JumpVariables

    #region GroundCollisionVariable
    [Header("Ground Collision Checks")]
    [SerializeField] private float _maxStepHeight = 1;
    [SerializeField] private LayerMask _groundMask;
    private int _stepsSinceLastGrounded = 0;
    private int _stepsSinceLastJump = 0;

    #endregion

    #region GravityVariables

    [Header("Gravity")]
    public Vector3 CustomGravityDirection = Vector3.down;
    public float CustomGravityForce = 9.81f;

    #endregion GravityVariables

    [Header("Debug")]
    [SerializeField] private Renderer _renderer;


    public Vector3 CurrentVelocity { get; private set; }

    #region InputVariables

    private bool _inputJump;
    public bool InputJump {
        get { return _inputJump; }
        set {
            _inputJump = value;
        }
    }

    private Vector2 _moveInput;
    public Vector2 MoveInput {
        get { return _moveInput; }
        set {
            _moveInput = Vector2.ClampMagnitude(value, 1);
        }
    }

    #endregion InputVariables

    #region PrivateVariable
    // PRIVATE VARIABLE
    private Vector3 _velocity;
    private Vector3 _desiredVelocity;
    private Vector3 _previousPosition;
    private Vector3 _groundNormal;
    private Vector3 _steepNormal;
    private int _groundContactCount;
    private int _steepContactCount;
    private float _maxDotFloorValue;
    private bool _isJumping = false;


    private bool _isGrounded;

    private Rigidbody _rb;

    #endregion PrivateVariable
    // FUNCTIONS

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        HandleDirection();

        HandleFalling();
    }

    void FixedUpdate()
    {
        UpdateState();
        AdjustVelocity();


        if (InputJump) {
            Jump();
        }

        TestingDebug();

        _rb.velocity = _velocity;
        ClearState();
    }

    #region Movement

    private void HandleDirection()
    {
        _desiredVelocity = new Vector3(MoveInput.x, 0, MoveInput.y);

        CurrentVelocity = (transform.position - _previousPosition) / Time.deltaTime;
        _previousPosition = transform.position;

        Transform directionTransform = playerInputSpace;

        Vector3 rotation = playerInputSpace.rotation.eulerAngles;
        rotation.x = 0;
        rotation.z = 0;

        directionTransform.rotation = Quaternion.Euler(rotation);


        _desiredVelocity = directionTransform.TransformDirection(_desiredVelocity);

        _desiredVelocity *= _maxSpeed;
    }

    void AdjustVelocity()
    {
        Vector3 xAxis = ProjectOnContactPlane(Vector3.right).normalized;
        Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized;

        float currentX = Vector3.Dot(_velocity, xAxis);
        float currentZ = Vector3.Dot(_velocity, zAxis);

        float acceleration = _isGrounded ? _maxAcceleration : _maxAirAcceleration;
        float maxSpeedChange = acceleration * Time.deltaTime;

        float newX =
            Mathf.MoveTowards(currentX, _desiredVelocity.x, maxSpeedChange);
        float newZ =
            Mathf.MoveTowards(currentZ, _desiredVelocity.z, maxSpeedChange);

        _velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
    }

    #endregion Movement

    #region Jump
    private void Jump()
    {
        if (_isGrounded) {
            _stepsSinceLastJump = 0;
            _isGrounded = false;

            float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * JumpHeight);

            if (_velocity.y > 0f) {
                jumpSpeed = Mathf.Max(jumpSpeed - _velocity.y, 0f);
            }

            _velocity.y += jumpSpeed;
        }
    }

    void HandleFalling()
    {
        if (!_isGrounded)
        {
            //Better Jump
            if (_rb.velocity.y < 0)
            {
                _rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (_rb.velocity.y > 0 && !InputJump)
            {
                _rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }
    }

    #endregion Jump

    #region OnGroundCollisionHandling
    private void OnCollisionEnter(Collision collision)
    {
        EvaluationCollision(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        EvaluationCollision(collision);
    }

    void EvaluationCollision(Collision collision)
    {
        _isGrounded = false;

        for (int i = 0; i < collision.contactCount; i++) {

            Vector3 normal = collision.contacts[i].normal;
            float upDot = Vector3.Dot(normal, Vector3.up);

            if (upDot >= _maxDotFloorValue) {
                _groundContactCount++;
                _groundNormal += normal;

            }
            else if (upDot > -0.01f) {
                _steepContactCount++;
                _steepNormal += normal;
            }

        }

        if (_groundContactCount == 0 && _steepContactCount > 0) {

            _steepNormal.Normalize();

            float upDot = Vector3.Dot(_steepNormal, Vector3.up);
            if (upDot >= _maxDotFloorValue) {

                _groundContactCount = 1;
                _groundNormal = _steepNormal;
            }
        }

        _groundNormal.Normalize();

        _isGrounded = _groundContactCount > 0;
    }

    void OnCollisionExit()
    {
        _isGrounded = false;
    }

    private void UpdateGravity(bool isGrounded)
    {
        if (isGrounded)
        {
            _rb.useGravity = false;
        }
        else
        {
            _rb.useGravity = true;
        }
    }

    private bool AdditionalGroundCheck(bool currentGrounded)
    {
        if (currentGrounded)
        {
            return currentGrounded;
        }
        bool isGrounded = currentGrounded;


        //First check, Raycast under

        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);

        Debug.DrawRay(transform.position, Vector3.down, Color.green);

        if(Physics.Raycast(ray, out hit, _maxStepHeight, _groundMask))
        {
            _groundNormal = hit.normal;
            _groundContactCount = 1;
            float speed = _velocity.magnitude;
            float dot = Vector3.Dot(_velocity, hit.normal);
            _velocity = (_velocity - hit.normal * dot).normalized * speed;
            //_rb.position = hit.point +  (_lastGroundNormal /2);
            isGrounded = true;
        }

        //Second check, Raycast in front-down


        return isGrounded;
    }

    #endregion OnGroundCollisionHandling


    private void Initialize()
    {
        _previousPosition = transform.position;
        _maxDotFloorValue = Mathf.Cos(_maxFloorAngle * Mathf.Deg2Rad);

        _rb = GetComponent<Rigidbody>();
    }
    private void UpdateState()
    {
        _stepsSinceLastGrounded += 1;
        _stepsSinceLastJump += 1;
        _velocity = _rb.velocity;


        if (_isGrounded || SnapToGround())
        {
            _stepsSinceLastGrounded = 0;
        }
        else
        {
            _groundNormal = Vector3.up;
        }
        UpdateGravity(_isGrounded);
    }
    private void ClearState()
    {
        _steepNormal = _groundNormal = Vector3.zero;
        _steepContactCount = _groundContactCount = 0;
    }

    

    private void TestingDebug()
    {
        Debug.DrawRay(transform.position, _velocity.normalized * 2, Color.red);
        Debug.DrawRay(transform.position, _groundNormal * 2, Color.blue);
        _renderer.material.SetColor(
            "_Color", _isGrounded ? Color.black : Color.white
        );
    }

    bool SnapToGround()
    {
        if(_stepsSinceLastGrounded > 1 || _stepsSinceLastJump <=2) // to only snap directly after leaving ground
        {
            return false;
        }


        if (!Physics.Raycast(_rb.position,Vector3.down, out RaycastHit hit, _maxStepHeight, _groundMask))
        {
            return false;
        }

        if(hit.normal.y < _maxDotFloorValue)
        {
            return false;
        }

        _groundContactCount = 1;
        _groundNormal = hit.normal;
        float speed = _velocity.magnitude;
        float dot = Vector3.Dot(_velocity, hit.normal);
        if(dot > 0)
        {
            _velocity = (_velocity - hit.normal * dot).normalized * speed;
        }
        return true;
    }

    Vector3 ProjectOnContactPlane(Vector3 vector)
    {
        return vector - _groundNormal * Vector3.Dot(vector, _groundNormal);
    }
}
