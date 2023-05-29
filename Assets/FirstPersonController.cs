using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonController : MonoBehaviour {

    GenericInputs _inputActions;
    [SerializeField] SimpleMovementController _mc;
    [SerializeField] FPVCamera _fpv;
    [SerializeField] FPSShoot _fPSShoot;

    // Start is called before the first frame update
    void Start()
    {
        _inputActions = new GenericInputs();
        _inputActions.Enable();

        _inputActions.Player.Move.performed += Move_performed;
        _inputActions.Player.Move.canceled += Move_performed;

        _inputActions.Player.Look.performed += Look_performed;
        _inputActions.Player.Look.canceled += Look_performed;

        _inputActions.Player.Fire.performed += Fire_performed;

    }

    private void Fire_performed(InputAction.CallbackContext obj)
    {
        _fPSShoot.Shoot();
    }

    private void Look_performed(InputAction.CallbackContext obj)
    {
        _fpv.AimInput = obj.performed ? obj.ReadValue<Vector2>() : Vector2.zero;
    }

    private void Move_performed(InputAction.CallbackContext obj)
    {
        _mc.MoveInput = obj.performed ? obj.ReadValue<Vector2>() : Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
