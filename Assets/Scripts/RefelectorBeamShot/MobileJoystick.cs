using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileJoystick : MonoBehaviour {


    //Joystick
    private bool _joystickInUse;

    public bool JoystickInUse {
        get { return _joystickInUse; }
        set {
            if (!value) {
                ResetJoystick();
            }

            _joystickInUse = value;
        }
    }

    public RectTransform JoystickZone;
    public RectTransform stick;

    public int JoystickTouchId = -1;
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach (Touch touch in Input.touches) {
            if (!JoystickInUse) {
                if (touch.phase == TouchPhase.Began) {
                    float distance = Vector2.Distance(JoystickZone.position, touch.position);

                    if (distance <= (JoystickZone.sizeDelta.x / 2)) {
                        JoystickInUse = true;
                        JoystickTouchId = touch.fingerId;
                        break;
                    }
                }
            }
        }

        if (JoystickInUse) {
            if (Input.touchCount > JoystickTouchId) {
                UseJoystick();

                if (Input.touches[JoystickTouchId].phase == TouchPhase.Ended) {
                    JoystickInUse = false;
                }
            }
            else {
                JoystickInUse = false;
            }
        }


    }

    public void UseJoystick()
    {
        stick.position = Input.touches[JoystickTouchId].position;

        direction = Vector3.ClampMagnitude((stick.position - JoystickZone.position) / (JoystickZone.sizeDelta.x / 2), 1f);
    }

    private void ResetJoystick()
    {
        stick.position = stick.parent.position;
        direction = Vector3.zero;
    }


    public Vector3 GetDirection()
    {
        return direction;
    }

}
