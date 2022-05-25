using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {

    [Header("Health")]
    [SerializeField]
    private Image _healthSlider;
    public Gradient HealthColor;
    public AnimationCurve HealthAdvancement;

    [Header("Money")]
    [SerializeField]
    private TextMeshProUGUI _moneyValue;

    [Header("Shop")]
    public GameObject ShopOrigin;
    public TextMeshProUGUI ShopPrompt;
    public TextMeshProUGUI ShopCostValue;

    [Header("Shop")]
    public GameObject GameOverOrigin;

    [Header("Audio")]
    public Sprite AudioOn;
    public Sprite AudioOff;
    public Image AudioImageBtn;
    private bool isMuted = false;

    [Header("Timer")]
    public TextMeshProUGUI TimerValue;

    [Header("GameOver")]
    public TextMeshProUGUI FinalTimerValue;

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

        FinalTimerValue.text = TimerValue.text;

    }

    private void OnMoneyChanged()
    {
        _moneyValue.text = playerTopDownShooter.MoneyAmount.ToString();
    }

    private void OnHpChanged()
    {
        float hpPercent = 1 - (float)playerHealth.Hp / playerHealth.MaxHp;

        float sliderValue = HealthAdvancement.Evaluate(hpPercent);

        _healthSlider.fillAmount = sliderValue;
        _healthSlider.color = HealthColor.Evaluate(sliderValue);
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

    public void UpdateTimer(float time)
    {
        int seconds = Mathf.FloorToInt(time) % 60;
        int minutes = Mathf.FloorToInt(time) / 60;

        TimerValue.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    public void ChangeMuteState()
    {
        if (!isMuted) {
            AudioListener.volume = 0;
            isMuted = true;
            AudioImageBtn.sprite = AudioOff;
        }
        else {
            AudioListener.volume = 1;
            isMuted = false;
            AudioImageBtn.sprite = AudioOn;
        }
    }

    public void OpenPauseMenu()
    {
        GameManager.Instance.PauseUI.OpenPauseMenu();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
