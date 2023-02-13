using UnityEngine;

public class player : MonoBehaviour {

    public enum MovementState {
        Idle,
        Walking,
        Crouching
    }

    private MovementState moveState = MovementState.Idle;

    public MovementState MoveState {
        get { return moveState; }
        set { moveState = value; }
    }

    public float walkSpeed = 5;
    public float crouchSpeed = 3;
    public float groundAcceleration = 2;
    public float airAcceleration = 1;
    public float jumpHeight = 2;
    public float fallMultiplier = 0.25f;
    public float slowFallMultiplier = 0.25f;

    public float friction = 5;

    public Weapon LongWeapon;
    public Weapon ShortWeapon;

    public Transform LongWpnPos;
    public Transform ShortWpnPos;
    public Transform HoldingWpnPos;


    Rigidbody2D rb;
    FacingController fc;
    float playerInputDirection;
    Vector2 velocity;
    bool onGround = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        fc = GetComponent<FacingController>();
    }

    // Update is called once per frame
    void Update()
    {
        velocity = rb.velocity;

        playerInputDirection = Input.GetAxisRaw("Horizontal");


        if (Input.GetButtonDown("Fire1")) {
            LongWeapon.ActivatePrincipalAction();
        }
        else if (Input.GetButtonUp("Fire1")) {
            LongWeapon.StopPrincipalAction();
        }


        if (playerInputDirection < 0) {
            fc.FacingSide = FacingController.FacingDirections.Left;
            LongWeapon.GetComponent<FacingController>().FacingSide = FacingController.FacingDirections.Left;
            ShortWeapon.GetComponent<FacingController>().FacingSide = FacingController.FacingDirections.Left;

        }
        else if (playerInputDirection > 0) {
            fc.FacingSide = FacingController.FacingDirections.Right;
            LongWeapon.GetComponent<FacingController>().FacingSide = FacingController.FacingDirections.Right;
            ShortWeapon.GetComponent<FacingController>().FacingSide = FacingController.FacingDirections.Right;
        }

        float speed = MoveState == MovementState.Crouching ? crouchSpeed : walkSpeed;

        float acceleration = onGround ? groundAcceleration : airAcceleration;
        float displacement = playerInputDirection * speed * acceleration * Time.deltaTime;
        velocity.x += displacement;
        velocity.x = Mathf.Clamp(velocity.x, -speed, speed);

        if (playerInputDirection == 0 && onGround) {
            velocity.x -= velocity.x * friction * Time.deltaTime;
        }

        #region Jump
        if (Input.GetButtonDown("Jump") && onGround) {
            velocity.y = Mathf.Sqrt(2f * jumpHeight * -Physics2D.gravity.y);
        }

        if (!Input.GetButton("Jump") && !onGround) {
            velocity.y -= fallMultiplier * Time.deltaTime;
        }
        else if (Input.GetButton("Jump") && velocity.y < 2) {
            velocity.y -= slowFallMultiplier * Time.deltaTime;

        }
        #endregion Jump
        #region Crouch

        //if (Input.GetButtonDown("Crouch") && onGround) {
        //    transform.localScale = new Vector3(1.25f, 1, 1);
        //    transform.position = transform.position - new Vector3(0, 0.5f, 0);

        //    MoveState = MovementState.Crouching;

        //}

        //if (Input.GetButtonUp("Crouch") && MoveState == MovementState.Crouching) {
        //    transform.localScale = new Vector3(1, 2, 1);
        //    transform.position = transform.position + new Vector3(0, 0.5f, 0);

        //    MoveState = MovementState.Walking;
        //}

        #endregion Crouch
        rb.velocity = velocity;


        #region Guns

        if (LongWeapon != null) {
            LongWeapon.transform.position = HoldingWpnPos.position;
            LongWeapon.transform.rotation = HoldingWpnPos.rotation;
        }
        if (ShortWeapon != null) {
            ShortWeapon.transform.position = ShortWpnPos.position;
            ShortWeapon.transform.rotation = ShortWpnPos.rotation;
        }


        #endregion Guns


    }


    private void FixedUpdate()
    {
        groundContactCount = 0;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        EvaluateCollision(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EvaluateCollision(collision);
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        onGround = false;
    }

    float groundContactCount = 0;

    private void EvaluateCollision(Collision2D collision)
    {

        for (int i = 0; i < collision.contactCount; i++) {

            Vector2 normal = collision.contacts[i].normal;

            float upDot = Vector2.Dot(Vector2.up, normal);

            if (upDot >= 0.7f) {
                groundContactCount++;
            }
        }

        onGround = groundContactCount != 0;


    }



}
