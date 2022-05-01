using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBouncePowerup : MonoBehaviour {

    public int Cost = 175;
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

                    firearm.ReboundCount += 1;
                    Cost = (int)(Cost * CostIncreaseMultiplier);

                    _gameUI.ShowShopPrompt("Bullet Bounce Upgrade", Cost, color);
                }
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            _playerInZone = true;
            _gameUI.ShowShopPrompt("Bullet Bounce Upgrade", Cost, color);
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
