using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConfiguration : MonoBehaviour
{
    public bool IsAssigned = false;
    public PlayerInput PlayerInput = null;
    InputDevice inputDevice;
    public int PlayerIndex = -1;

    public void Setup(PlayerInput playerInput, int playerIndex)
    {
        PlayerInput = playerInput;
        inputDevice = PlayerInput.devices[0];
        this.PlayerIndex = playerIndex;
        IsAssigned = true;
    }

}
