using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private static GameManager _instance;

    public static GameManager Instance {
        get {
            if (_instance == null) {

                GameObject gameManagerGameObject = Resources.Load<GameObject>("GameManager");
                GameObject managerObject = Instantiate(gameManagerGameObject);
                _instance = managerObject.GetComponent<GameManager>();
                _instance.Initialize();

                DontDestroyOnLoad(_instance);

            }
            return _instance;
        }
    }

    public GameObject Player { get; private set; }
    public GameObject CameraObject { get; private set; }
    public AudioSource AudioManager { get; private set; }
    public GameUI GameUI { get; private set; }
    public PauseUI PauseUI { get; private set; }

    private void Initialize()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        OnSceneLoaded();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        OnSceneLoaded();
    }

    private void OnSceneLoaded()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        GameUI = GameObject.FindGameObjectWithTag("GameUI").GetComponent<GameUI>();
        PauseUI = GameObject.Find("PauseUI").GetComponent<PauseUI>();
        AudioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioSource>();
        CameraObject = Camera.main.gameObject;
    }

}
