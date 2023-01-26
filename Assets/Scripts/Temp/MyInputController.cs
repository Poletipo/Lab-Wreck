using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MyInputController : MonoBehaviour
{
    public static List<GameObject> DevicesInputList = new List<GameObject>();
    public static List<PlayerConfiguration> PlayerConfigList = new List<PlayerConfiguration>();
    private PlayerInputManager playerInputManager;

    public bool inGameSpawning = true;

    // Start is called before the first frame update
    void Start()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        playerInputManager.onPlayerJoined += PlayerInputManager_onPlayerJoined;
    }

    /// <summary>
    /// Spawns Input GameObjects for the controller that connects
    /// </summary>
    /// <param name="obj"></param>
    private void PlayerInputManager_onPlayerJoined(PlayerInput obj)
    {
        DevicesInputList.Add(obj.gameObject);
        Debug.Log($"Device joined: {obj.devices[0].name}");
        obj.transform.parent = transform;

        if (inGameSpawning)
        {
            PlayerConfiguration tmpPlayerConfig = obj.gameObject.GetComponent<PlayerConfiguration>();

            tmpPlayerConfig.Setup(obj, PlayerConfigList.Count);
            PlayerConfigList.Add(tmpPlayerConfig);

            GameManager.Instance.Spawner.Spawn(tmpPlayerConfig);
            Debug.Log("input controller spawn");
        }

    }





}
