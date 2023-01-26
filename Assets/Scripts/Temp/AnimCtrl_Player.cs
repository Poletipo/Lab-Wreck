using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AnimCtrl_Player : MonoBehaviour {

    public enum AnimStates {
        Idle,
        Run,
        Jump,
        Falling,
        Attack,
        Land,
        Hurt,

        Null
    }

    private AnimStates _animState = AnimStates.Idle;

    public AnimStates AnimState {
        get { return _animState; }
        set {
            _animState = value;
            animator.Play(CharacterName + "_" + AnimState.ToString() + "_Anim");
        }
    }

    public string CharacterName = "";

    private bool _isJumping = false;
    private bool _isFalling = false;

    Animator animator;
    MovementController mc;
    public IDamager hitter;
    SpriteRenderer spr;
    private bool _isAttacking;

    // Start is called before the first frame update
    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        mc = GetComponent<MovementController>();

        hitter.OnAttack += OnAttack;

        mc.OnJumping += OnJumping;
        mc.OnFalling += OnFalling;
    }

    private void OnAttack(IDamager hitter)
    {
        _isAttacking = true;
    }

    private void OnFalling(MovementController mc)
    {
        _isFalling = true;
    }

    private void OnJumping(MovementController mc)
    {
        _isJumping = true;
    }

    // Update is called once per frame
    void Update()
    {
        switch (AnimState) {

            case AnimStates.Idle:
                IdleState();

                break;
            case AnimStates.Run:
                RunState();
                break;
            case AnimStates.Jump:
                JumpState();
                break;
            case AnimStates.Falling:
                FallingState();
                break;
            case AnimStates.Attack:
                AttackingState();

                break;
        }
    }

    private void AttackingState()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !animator.IsInTransition(0)) {
            AnimState = AnimStates.Idle;
            _isAttacking = false;
        }
    }

    private void FallingState()
    {
        if (mc._isGrounded) {
            AnimState = AnimStates.Idle;
            _isFalling = false;
        }

        if (_isAttacking) {
            AnimState = AnimStates.Attack;
        }

    }

    private void JumpState()
    {
        if (_isFalling) {
            AnimState = AnimStates.Falling;
            _isJumping = false;
        }

        if (_isAttacking) {
            AnimState = AnimStates.Attack;
        }

    }

    void IdleState()
    {
        if (_isJumping) {
            AnimState = AnimStates.Jump;
        }

        if (_isAttacking) {
            AnimState = AnimStates.Attack;
        }

        if (mc.MoveInput.x != 0) {
            AnimState = AnimStates.Run;
        }
    }

    void RunState()
    {
        if (_isJumping) {
            AnimState = AnimStates.Jump;
        }

        if (_isFalling) {
            AnimState = AnimStates.Falling;
        }

        if (_isAttacking) {
            AnimState = AnimStates.Attack;
        }

        if (mc.MoveInput.x == 0) {
            AnimState = AnimStates.Idle;
        }
    }



}
