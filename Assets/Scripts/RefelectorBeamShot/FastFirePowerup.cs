using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastFirePowerup : MonoBehaviour {

    public int Cost = 100;
    public float CostIncreaseMultiplier = 1.5f;
    public float FireRateIncreaseMultiplier = 1.2f;
    public Color color;
    public AudioClip BuySound;
    public AudioClip DenySound;

    private bool _playerInZone = false;
    private GameObject _player;
    private GameUI _gameUI;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameManager.Instance.Player;
        _gameUI = GameManager.Instance.GameUI;
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerInZone) {
            if (Input.GetButtonDown("Interact")) {

                bool validPayment = _player.GetComponent<TopDownShooter>().Pay(Cost);

                if (validPayment) {
                    GameManager.Instance.AudioManager.PlayOneShot(BuySound);
                    Firearm firearm = _player.GetComponent<Firearm>();
                    firearm.fireRateSpeed /= FireRateIncreaseMultiplier;
                    Cost = (int)(Cost * CostIncreaseMultiplier);
                    _gameUI.ShowShopPrompt("Fire Rate Upgrade", Cost, color);
                }
                else {
                    GameManager.Instance.AudioManager.PlayOneShot(DenySound);
                }

            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            _playerInZone = true;
            _gameUI.ShowShopPrompt("Fire Rate Upgrade", Cost, color);
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
