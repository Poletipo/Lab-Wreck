using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneTest : MonoBehaviour
{

    public string sceneName;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }

    }
}
