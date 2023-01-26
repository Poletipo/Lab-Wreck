using UnityEngine;

public class Player : MonoBehaviour {

    private GameManager gameManager;
    private InputManager inputManager;

    private MovementController movementController;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();

        inputManager.OnStartJump += Jump;
        inputManager.OnEndJump += Jump;
        inputManager.OnMove += Move;
    }

    private void Jump(InputManager InputManager)
    {
        movementController.InputJump = InputManager.JumpInput;
    }

    private void Move(InputManager InputManager)
    {
        movementController.MoveInput = InputManager.MoveInput;
    }

    private void Initialize()
    {
        movementController = GetComponent<MovementController>();
        gameManager = GameManager.Instance;
        inputManager = gameManager.InputManager;
    }


}
