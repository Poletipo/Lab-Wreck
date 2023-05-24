using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DockInputManager : MonoBehaviour {

    DockInputs _inputs;
    [SerializeField] ClicMovement _clicMovement;

    // Start is called before the first frame update
    void Start()
    {
        _inputs = new DockInputs();
        _inputs.Enable();

        _inputs.Player.Select.performed += Select_performed;

    }

    private void Select_performed(InputAction.CallbackContext obj)
    {
        Vector2 mousePosition = _inputs.Player.MousePosition.ReadValue<Vector2>();
        _clicMovement.SelectMovePosition(mousePosition);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
