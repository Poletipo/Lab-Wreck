using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour {

    Rigidbody2D rb;
    float speed = 3;
    float inputDirections = 0;
    Vector2 localGravity = Vector2.down;
    float gravityForce = 9.81f;
    float currentAngle = 0;

    public AnimationCurve jumpCurve;
    private bool isJumping;
    private float jumpingTime = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        inputDirections = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump")) {
            isJumping = true;
        }


        if (isJumping) {
            Jumping();
        }


    }

    private void Jumping()
    {
        float curveValue = jumpCurve.Evaluate(jumpingTime);
        jumpingTime += Time.deltaTime;
        Debug.Log(curveValue);
    }

    void FixedUpdate()
    {
        rb.AddForce(localGravity * gravityForce * Time.deltaTime, ForceMode2D.Impulse);
        Vector2 rightDirection = -new Vector2(localGravity.y, -localGravity.x);
        rb.AddForce(inputDirections * rightDirection * Time.deltaTime * speed, ForceMode2D.Impulse);

        float desiredAngle = Mathf.Atan2(localGravity.y, localGravity.x) * Mathf.Rad2Deg + 90;

        currentAngle = Mathf.LerpAngle(currentAngle, desiredAngle, 10 * Time.deltaTime);
        Debug.Log(desiredAngle + " : " + currentAngle);
        if (currentAngle > 360) {
            currentAngle -= 360;
        }
        else if (currentAngle < -360) {
            currentAngle += 360;
        }


        rb.SetRotation(currentAngle);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        localGravity = Vector2.zero;

        for (int i = 0; i < collision.contactCount; i++) {
            localGravity += collision.contacts[i].normal;
        }

        localGravity = -localGravity.normalized;
    }


}
