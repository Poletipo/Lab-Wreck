using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public GameObject[] diceTypes;
    MeshCollider meshCollider;
    Rigidbody rb;

    public GameObject dice;
    public int diceIndex = 0;
    public int diceValue = 1;

    MyInputs inputActions;
    Vector2 MoveInput;

    [Header("Camera Target")]
    public Transform cameraTarget;

    [Header("Movement")]
    public float TorquePower = 25f;
    public float MovePower = 0.5f;
    public Transform InputSpace;
    public float jumpForce = 5f;
    private bool isMoving = false;
    private bool canJump = true;

    //SHOOTING
    [Header("Shooting")]
    bool isShooting = false;
    public GameObject Projectile;
    public float ShootInterval = .5f;
    float ShootIntervalTimer = 999f;

    [Header("Stun")]
    bool isStunned = false;
    public float StunnedTime = .2f;
    float stunnedTimer = .2f;

    [Header("Pause")]
    public PauseUI pauseUI;

    public GameObject Smoke;

    // Start is called before the first frame update
    void Awake()
    {
        inputActions = new MyInputs();

        inputActions.Player.Move.performed += ctx => Move(ctx);
        inputActions.Player.Move.canceled += ctx => CancelMove(ctx);
        
        //inputActions.Player.Test01.started += ctx => Test01();
        //inputActions.Player.Test02.started += ctx => Test02();

        inputActions.Player.Jump.performed += ctx => Jump();

        inputActions.Player.Shoot.started += ctx => StartShooting();
        inputActions.Player.Shoot.canceled += ctx => StopShooting();

        inputActions.Player.Pause.performed += ctx => Pause();

        meshCollider = GetComponent<MeshCollider>();
        rb = GetComponent<Rigidbody>();

        rb.maxAngularVelocity = 20;

        UpdateDice(diceIndex);

        Cursor.lockState = CursorLockMode.Locked;


    }

    private void OnDestroy()
    {
        inputActions.Player.Move.performed -= ctx => Move(ctx);
        inputActions.Player.Move.canceled -= ctx => CancelMove(ctx);

        //inputActions.Player.Test01.started -= ctx => Test01();
        //inputActions.Player.Test02.started -= ctx => Test02();

        inputActions.Player.Jump.performed -= ctx => Jump();

        inputActions.Player.Shoot.started -= ctx => StartShooting();
        inputActions.Player.Shoot.canceled -= ctx => StopShooting();

        inputActions.Player.Pause.performed -= ctx => Pause();
    }

    private void Pause()
    {
        pauseUI.OpenPauseMenu();
    }

    #region shooting
    private void StartShooting()
    {
        isShooting = true;
    }
    private void StopShooting()
    {
        isShooting = false;
    }
    #endregion shooting

    #region tests
    void Test01()
    {
        DowngradeDice();
    }
    private void Test02()
    {
        UpgradeDice();
    }
    #endregion tests


    private void Jump()
    {
        if (rb != null)
        {
            if (canJump)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                canJump = false;
            }
        }

    }

    private void CancelMove(InputAction.CallbackContext ctx)
    {
        MoveInput = Vector2.zero;
        isMoving = false;
    }

    private void Move(InputAction.CallbackContext ctx)
    {
        isMoving = true;
        MoveInput = ctx.ReadValue<Vector2>();

    }

    public void SetDiceValue()
    {
        diceValue = dice.GetComponent<Dice>().GetFaceUpValue();
    }

    public void UpgradeDice()
    {
        if(diceIndex+1 < diceTypes.Length)
        {
            diceIndex++;
            UpdateDice(diceIndex);
        }
    }

    public void DowngradeDice()
    {
        if(diceIndex-1 >= 0)
        {
            diceIndex--;
            UpdateDice(diceIndex);
        }

        if (!isStunned)
        {
            isStunned = true;
            stunnedTimer = 0;
        }

    }

    void UpdateDice(int index)
    {
        if(dice != null)
        {
            Destroy(dice);
        }
        meshCollider.sharedMesh = diceTypes[index].GetComponent<Dice>().meshCollider;

        dice = Instantiate(diceTypes[index], transform);
        Instantiate(Smoke, transform.position, Quaternion.identity);

    }
    private void OnEnable()
    {
        inputActions.Player.Enable();
    }


    private void FixedUpdate()
    {
        cameraTarget.position = transform.position + Vector3.up;
    }

    // Update is called once per frame
    void Update()
    {
        SetDiceValue();


        //Moving
        if (isMoving)
        {
            Vector3 toMoveForward = Vector3.Cross(InputSpace.forward, Vector3.up);

            rb.AddForce(toMoveForward * MovePower * - MoveInput.x);
            rb.AddForce(InputSpace.forward * MovePower * MoveInput.y);


            rb.AddTorque(toMoveForward * (-MoveInput.y * TorquePower), ForceMode.Acceleration);
            rb.AddTorque(InputSpace.forward * (-MoveInput.x * TorquePower), ForceMode.Acceleration);
        }

        //Shooting
        if (isShooting)
        {
            ShootIntervalTimer += Time.deltaTime;

            if(ShootIntervalTimer>= ShootInterval && !isStunned)
            {
                //GameObject project = Instantiate(Projectile, transform.position, Quaternion.identity);

                GameObject project = PoolManager.GetPoolObject(Projectile);
                project.SetActive(true);
                project.GetComponent<Projectile>().Setup(transform.position, Camera.main.transform.forward+ Vector3.up*0.1f, diceValue);

                ShootIntervalTimer = 0;
            }

        }

        if (isStunned)
        {
            stunnedTimer += Time.deltaTime;

            if(stunnedTimer >= StunnedTime)
            {
                isStunned = false;
            }

        }


    }

    private void OnCollisionEnter(Collision collision)
    {

        float upDot = Vector3.Dot(collision.contacts[0].normal, Vector3.up);

        if (upDot >= 0.5f)
        {
            canJump = true;
        }


    }


}
