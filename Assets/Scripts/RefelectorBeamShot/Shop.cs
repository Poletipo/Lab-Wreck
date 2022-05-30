using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Shop : MonoBehaviour {
    public int Cost;
    public float CostIncreaseMultiplier = 1.5f;
    public Color color;
    public AudioClip BuySound;
    public AudioClip DenySound;

    public bool _playerInZone = false;
    public GameObject _player;
    public GameUI _gameUI;

    public string stringPrompt;

    abstract public void Upgrade();

    // Update is called once per frame
    void Update()
    {
        if (_playerInZone) {
            if (Input.GetButtonDown("Interact")) {

                TryShopping();
            }
        }
    }

    public bool TryShopping()
    {

        if (_player == null) {
            _player = GameManager.Instance.Player;
        }
        bool validPayment = _player.GetComponent<TopDownShooter>().Pay(Cost);

        if (validPayment) {
            GameManager.Instance.AudioManager.PlayOneShot(BuySound);
            Upgrade();

            _gameUI.ShowShopPrompt(stringPrompt, Cost, color, this);
        }
        else {
            GameManager.Instance.AudioManager.PlayOneShot(DenySound);
        }
        return validPayment;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {

            _playerInZone = true;

            if (_gameUI == null) {
                _gameUI = GameManager.Instance.GameUI;
            }

            _gameUI.ShowShopPrompt(stringPrompt, Cost, color, this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") {
            _playerInZone = false;

            _gameUI.HideShopPrompt();
        }
    }




}
