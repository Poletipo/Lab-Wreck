using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {

    [Header("Health")]
    [SerializeField]
    private Image _healthSlider;
    public Gradient HealthColor;

    [Header("Money")]
    [SerializeField]
    private TextMeshProUGUI _moneyValue;

    [Header("Shop")]
    public GameObject ShopOrigin;
    public TextMeshProUGUI ShopPrompt;
    public TextMeshProUGUI ShopCostValue;

    [Header("Shop")]
    public GameObject GameOverOrigin;

    private GameObject _player;
    private Health playerHealth;
    private TopDownShooter playerTopDownShooter;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameManager.Instance.Player;

        playerHealth = _player.GetComponent<Health>();
        playerHealth.OnHpChanged += OnHpChanged;
        playerHealth.OnDeath += OnDeath;

        playerTopDownShooter = _player.GetComponent<TopDownShooter>();
        playerTopDownShooter.OnMoneyChanged += OnMoneyChanged;
    }

    private void OnDeath()
    {
        GameOverOrigin.SetActive(true);
    }

    private void OnMoneyChanged()
    {
        _moneyValue.text = playerTopDownShooter.MoneyAmount.ToString();
    }

    private void OnHpChanged()
    {
        _healthSlider.fillAmount = 1 - (float)playerHealth.Hp / playerHealth.MaxHp;
        _healthSlider.color = HealthColor.Evaluate(_healthSlider.fillAmount);
    }

    public void ShowShopPrompt(string prompt, int cost, Color color)
    {
        ShopCostValue.text = cost.ToString();
        ShopPrompt.text = prompt;
        ShopPrompt.color = color;
        ShopOrigin.SetActive(true);
    }

    public void HideShopPrompt()
    {
        ShopOrigin.SetActive(false);
    }

    public void HideUI()
    {
        GetComponent<CanvasGroup>().alpha = 0;
    }

    public void ShowUI()
    {
        GetComponent<CanvasGroup>().alpha = 1;
    }


    // Update is called once per frame
    void Update()
    {

    }
}
