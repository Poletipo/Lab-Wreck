using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.Utilities;

public class PlayerConnection : MonoBehaviour
{
    List<PlayerInput> playersInput = new List<PlayerInput>();

    public InputActionProperty joinAction;
    public GameObject playerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
        if (joinAction.reference != null && joinAction.action?.actionMap?.asset != null)
        {
            InputActionAsset inputActionAsset = Instantiate(joinAction.action.actionMap.asset);
            InputActionReference inputActionReference = InputActionReference.Create(inputActionAsset.FindAction(joinAction.action.name));
            joinAction = new InputActionProperty(inputActionReference);
        }

        joinAction.action.performed += JoinGame;
        joinAction.action.Enable();
    }

    private void OnEnable()
    {
        InputUser.onUnpairedDeviceUsed += UnPairedDeviceUsed;
        ++InputUser.listenForUnpairedDeviceActivity;
    }

    private void JoinGame(InputAction.CallbackContext ctx)
    {
        InputDevice device = ctx.control.device;
        if (PlayerInput.FindFirstPairedToDevice(device) == null)
        {
            playersInput.Add(PlayerInput.Instantiate(prefab: playerPrefab, playerIndex: playersInput.Count, splitScreenIndex: -1,
                controlScheme: null, pairWithDevice: device));
        }

    }

    private void UnPairedDeviceUsed(InputControl arg1, InputEventPtr arg2)
    {
        PlayerInput.Instantiate(prefab: playerPrefab, pairWithDevice: arg1.device);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
