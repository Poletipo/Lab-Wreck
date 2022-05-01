using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCountPowerup : MonoBehaviour {

    public int Cost = 150;
    public float CostIncreaseMultiplier = 1.5f;
    public Color color;

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
                    Firearm firearm = _player.GetComponent<Firearm>();

                    firearm.nbBulletPerShot += 1;
                    firearm.maxAngleOffset += 5;
                    Cost = (int)(Cost * CostIncreaseMultiplier);

                    _gameUI.ShowShopPrompt("Bullet Count Upgrade", Cost, color);
                }
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            _playerInZone = true;
            _gameUI.ShowShopPrompt("Bullet Count Upgrade", Cost, color);
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
