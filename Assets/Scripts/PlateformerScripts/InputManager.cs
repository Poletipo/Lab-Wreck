using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool JumpInput { get; private set; }



    public delegate void InputEvent(InputManager InputManager);
    public InputEvent OnStartJump;
    public InputEvent OnEndJump;
    public InputEvent OnMove;
    public InputEvent OnLook;

    private PlayerAction controls;

    // Start is called before the first frame update
    void Awake()
    {
        controls = new PlayerAction();

        controls.Gameplay.Jump.started += ctx => StartJump();
        controls.Gameplay.Jump.canceled += ctx => EndJump();

        controls.Gameplay.Move.performed += ctx => Move(ctx);
        controls.Gameplay.Move.canceled += ctx => Move(ctx);

        controls.Gameplay.Look.performed += ctx => Look(ctx);
        controls.Gameplay.Look.canceled += ctx => Look(ctx);

    }

    private void Look(InputAction.CallbackContext ctx)
    {
        LookInput = ctx.ReadValue<Vector2>();
        OnLook?.Invoke(this);
    }

    private void Init()
    {

    }

    private void Move(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
        OnMove?.Invoke(this);
    }

    private void StartJump()
    {
        JumpInput = true;
        OnStartJump?.Invoke(this);
    }

    private void EndJump()
    {
        JumpInput = false;
        OnEndJump?.Invoke(this);
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }


}
