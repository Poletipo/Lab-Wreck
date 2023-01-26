using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCtrl_Test : MonoBehaviour
{
    public Health hp;
    public MovementController mc;
    public PlayerController pc;
    public Animator anim;

    float speed = 0;

    // Start is called before the first frame update
    void Start()
    {
        mc.OnLanding += OnLanding;
        mc.OnJumping += OnJumping;
        mc.OnCrouchingChange += OnCrouchingChange;
        pc.OnAttack += OnAttack;

        hp.OnHurt += OnHurt;

    }

    private void OnHurt()
    {
        anim.SetTrigger("HurtTrigger");
    }

    private void OnCrouchingChange(MovementController mc)
    {
        anim.SetBool("IsCrouched", mc.IsCrouched);
    }

    private void OnAttack(int obj)
    {
        anim.SetTrigger("Attack");
    }

    private void OnJumping(MovementController mc)
    {
        anim.SetTrigger("JumpTrigger");
        anim.SetBool("IsInAir", true);
    }

    private void OnLanding(MovementController mc)
    {
        anim.SetBool("IsInAir", false);
    }

    float vel;
    // Update is called once per frame
    void Update()
    {

        



        speed = Mathf.SmoothDamp(speed, Mathf.Abs(mc.Velocity.x) / mc.CurrentSpeed, ref vel, 0.1f);

        anim.SetFloat("Velocity", speed);

    }
}
