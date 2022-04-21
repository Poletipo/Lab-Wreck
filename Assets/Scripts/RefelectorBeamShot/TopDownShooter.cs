using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownShooter : MonoBehaviour {


    public float Speed = 5f;

    private bool isController = false;

    private Vector3 controllerDir;

    private Plane _plane = new Plane(Vector3.up, 0);

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //transform.position += transform.forward * Time.deltaTime;

        Move();



        Look();

    }

    private void Move()
    {
        rb.velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * Speed;
    }

    private void Look()
    {
        controllerDir = new Vector3(-Input.GetAxisRaw("RightStickX"), Input.GetAxisRaw("RightStickY"), 0);

        if (controllerDir.magnitude > 0) {
            isController = true;
        }
        else if (Input.GetAxisRaw("Mouse X") > 0 || Input.GetAxisRaw("Mouse Y") > 0) {
            isController = false;
        }

        if (isController && controllerDir.magnitude > 0) {
            Vector3 mousePosition = controllerDir;
            float lookAtAngle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(lookAtAngle - 90, Vector3.up);
        }
        else if (!isController) {
            Vector3 worldMousePosition;
            float distance;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (_plane.Raycast(ray, out distance)) {
                worldMousePosition = ray.GetPoint(distance);

                Vector3 mousePosition = worldMousePosition - transform.position;

                Debug.DrawRay(transform.position, mousePosition);

                float lookAtAngle = Mathf.Atan2(mousePosition.z, mousePosition.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(lookAtAngle - 90, Vector3.down);
            }
        }


    }


}
