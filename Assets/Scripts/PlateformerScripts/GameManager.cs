using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private static GameManager _instance;

    public static GameManager Instance {
        get {
            // TODO: use a bootloader instead to create this before level is started since it can be expensive to load all assets
            if (_instance == null) {
                _instance = GameObject.FindObjectOfType<GameManager>();

                if (_instance == null) {
                    Debug.Log("Gamemanager null");
                    GameObject gameManagerGameObject = Resources.Load<GameObject>("GameManager");
                    GameObject managerObject = Instantiate(gameManagerGameObject);
                    _instance = managerObject.GetComponent<GameManager>();
                }
                _instance.Initialize();

                // Prevents having to recreate the manager on scene change
                // https://docs.unity3d.com/ScriptReference/Object.DontDestroyOnLoad.html
                DontDestroyOnLoad(_instance);
            }

            return _instance;
        }
    }

    public InputManager InputManager { get; private set; }

    private void Initialize()
    {
        InputManager = GetComponentInChildren<InputManager>();

        SceneManager.sceneLoaded += OnSceneLoaded;

        OnSceneLoaded();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        OnSceneLoaded();
    }

    private void OnSceneLoaded()
    {
        // Do something...
    }


}
