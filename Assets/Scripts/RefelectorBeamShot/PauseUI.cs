using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour {

    public GameObject PauseMenuOrigin;
    private bool _pauseMenuOpen = false;
    private TopDownShooter _player;
    private GameUI _gameUI;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameManager.Instance.Player.GetComponent<TopDownShooter>();
        _gameUI = GameManager.Instance.GameUI;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("PauseBtn")) {

            if (_pauseMenuOpen) {
                ClosePauseMenu();
            }
            else {
                OpenPauseMenu();
            }

        }
    }

    public void OpenPauseMenu()
    {
        PauseMenuOrigin.SetActive(true);
        _pauseMenuOpen = true;
        Time.timeScale = 0;
        _player.InputEnabled = false;
        _gameUI.HideUI();
    }

    public void ClosePauseMenu()
    {
        PauseMenuOrigin.SetActive(false);
        _pauseMenuOpen = false;
        Time.timeScale = 1;
        _player.InputEnabled = true;
        _gameUI.ShowUI();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
