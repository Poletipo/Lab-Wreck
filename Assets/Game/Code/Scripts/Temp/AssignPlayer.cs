using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class AssignPlayer : MonoBehaviour
{
    public GameObject loggedInObject;
    public Transform verticalLayout;

    public InputActionProperty joinAction;
    public InputActionProperty startAction;

    // Start is called before the first frame update
    void Start()
    {
        if (joinAction.reference != null && joinAction.action?.actionMap?.asset != null)
        {
            InputActionAsset inputActionAsset = Instantiate(joinAction.action.actionMap.asset);
            InputActionReference inputActionReference = InputActionReference.Create(inputActionAsset.FindAction(joinAction.action.name));
            joinAction = new InputActionProperty(inputActionReference);
        }

        startAction.action.performed += StartGame;

        joinAction.action.performed += JoinAction;
        joinAction.action.Enable();
    }

    private void StartGame(InputAction.CallbackContext obj)
    {
        SceneManager.LoadScene(1);
    }

    private void JoinAction(InputAction.CallbackContext ctx)
    {
        InputDevice device = ctx.control.device;
        PlayerInput tmpInput = PlayerInput.FindFirstPairedToDevice(device);
        PlayerConfiguration tmpPlayerConfig = tmpInput.gameObject.GetComponent<PlayerConfiguration>();

        if (!tmpPlayerConfig.IsAssigned)
        {
            Debug.Log($"{tmpInput.name} joined");
            tmpPlayerConfig.Setup(tmpInput, MyInputController.PlayerConfigList.Count);
            MyInputController.PlayerConfigList.Add(tmpPlayerConfig);

            GameObject tmp = Instantiate(loggedInObject);
            tmp.transform.SetParent(verticalLayout);

            tmp.GetComponent<LoggedInfo>().Setup(device.name, tmpPlayerConfig.PlayerIndex);

            GameManager.Instance.Spawner.Spawn(tmpPlayerConfig);
        }
    }
}
