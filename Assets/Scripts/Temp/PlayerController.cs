using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour {

    public Action<int> OnAttack;

    public Camera _cam;
    public GameObject virtualCam;
    public TextMeshProUGUI PlayerTxt;
    public GameObject bloodParticle;

    public PlayerConfiguration _playerConfig;
    private MovementController mc;
    private Health health;

    Vector2 moveInput = new Vector2();
    bool jumpInput = false;
    bool crouchInput = false;

    //private AetherInputAction _inputActions = null;

    //public AetherInputAction InputActions {
    //    get {

    //        if (_inputActions == null) {
    //            _inputActions = new AetherInputAction();
    //        }

    //        return _inputActions;
    //    }
    //    set { _inputActions = value; }
    //}

    PlayerInput _playerInput;

    // Start is called before the first frame update
    void Start() {
        mc = GetComponent<MovementController>();
        health = GetComponent<Health>();

        health.OnHurt += OnHurt;

        //InputActions.InGame.Move.performed += ctx => Move(ctx);
        //InputActions.InGame.Move.canceled += ctx => Move(ctx);

        // A nice example
        //playerInput = GetComponent<PlayerInput>();
        //fireAction = playerInput.currentActionMap.FindAction("Fire");

        //fireAction.performed += FireAction_performed;
        //fireAction.canceled += FireAction_canceled;
        //fireAction.started += FireAction_started;

        //moveAction = playerInput.currentActionMap.FindAction("move");

    }

    private void OnHurt() {

    }

    private void OnLanding(MovementController mc) {
        //animator.SetBool("IsInAir", false);
    }

    private void OnJump(MovementController mc) {
        //animator.SetTrigger("JumpTrigger");
        //animator.SetBool("IsInAir", true);
    }

    public void Setup(PlayerConfiguration playerConfig) {
        _playerConfig = playerConfig;
        _playerInput = playerConfig.PlayerInput;
        _playerInput.enabled = true;
        _playerInput.SwitchCurrentActionMap("InGame");
        _playerInput.onActionTriggered += ctx => HandleInputs(ctx);
    }

    public void UpdateScreen(Rect camRect) {
        int layer = 30 - _playerConfig.PlayerIndex;

        virtualCam.layer = layer;

        int bitMask = (1 << layer)
             | (1 << 0)
             | (1 << 1)
             | (1 << 2)
             | (1 << 4)
             | (1 << 5)
             | (1 << 8)
             | (1 << 9);

        _cam.cullingMask = bitMask;
        _cam.gameObject.layer = layer;
        _cam.rect = camRect;

        PlayerTxt.text = $"Player: {_playerConfig.PlayerIndex}";
    }

    private void HandleInputs(InputAction.CallbackContext ctx) {
        InputAction moveAction = _playerInput.currentActionMap.FindAction("Move");
        InputAction jumpAction = _playerInput.currentActionMap.FindAction("Jump");
        InputAction punchAction = _playerInput.currentActionMap.FindAction("Fire1");
        InputAction crouchAction = _playerInput.currentActionMap.FindAction("Crouch");

        if (ctx.action == moveAction) {
            moveInput.x = ctx.ReadValue<Vector2>().x;
            moveInput.y = ctx.ReadValue<Vector2>().y;
        }
        else if (ctx.action == jumpAction) {

            if (ctx.action.IsPressed()) {
                jumpInput = true;
            }
            else if (!ctx.action.IsPressed()) {
                jumpInput = false;
            }
        }
        else if (ctx.action == punchAction) {
            if (ctx.action.IsPressed()) {
                OnAttack?.Invoke(0);
            }
        }
        else if (ctx.action == crouchAction) {
            if (ctx.action.IsPressed()) {
                crouchInput = true;
            }
        }


    }

    // Update is called once per frame
    void Update() {
        HandleInput();
    }

    void HandleInput() {
        //mc.MoveInput = moveInput;
        //mc.JumpInput = jumpInput;

        //if (crouchInput)
        //{
        //    crouchInput = false;
        //    mc.IsCrouched = !mc.IsCrouched;
        //}
    }

}
