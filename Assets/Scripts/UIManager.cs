using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SocialPlatforms.Impl;
using UnityEditor.U2D;


public class UIManager : MonoBehaviour
{
    public Player player;

    [Header("Button Setup")]
    [SerializeField] private Button startButton;
    [SerializeField] public Button restartButton;
    [SerializeField] private Button shootButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button exitButton;

    [Header("UI Setup")]
    [SerializeField] private TMP_Text greetingText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text TimesCount;
    [SerializeField] private TMP_Text Moneys;
    [SerializeField] private TMP_Text HP;


    [SerializeField] private Image crosshair;
    [SerializeField] private Image gun;

    public static event Action OnUIRestartButton;
    public static event Action OnUIStartButton;
    public static event Action OnUIShootButton;
    public static event Action OnUIShopButton;
    public static event Action OnUIUpGradeButton;
    public static event Action OnUIReturnButton;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gun.sprite = player.currentWeapon.weaponIcon;

        startButton.onClick.AddListener(StartButtonPressed);
        shootButton.onClick.AddListener(ShootButtonPressed);

        shootButton.gameObject.SetActive(false);
        crosshair.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        TimesCount.gameObject.SetActive(false);
        Moneys.gameObject.SetActive(false);
        HP.gameObject.SetActive(false);

        shopButton.gameObject.SetActive(false);
        gun.gameObject.SetActive(false);
    }

    void StartButtonPressed()
    {
        OnUIStartButton?.Invoke();
        greetingText.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false); 
        shootButton.gameObject.SetActive(true);

        crosshair.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        scoreText.text = $"SCORE: 0";

        TimesCount.gameObject.SetActive(true);
        Moneys.gameObject.SetActive(true);
        HP.gameObject.SetActive(true);

        UpdateTime(0);
        UpdateCoin();
        UpdateHP();

        shopButton.gameObject.SetActive(false);

        settingButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
    }

    public void RestartButtonPressed()
    {
        OnUIRestartButton?.Invoke();

        greetingText.gameObject.SetActive(true);
        startButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(false);
        shootButton.gameObject.SetActive(false);
        shopButton.gameObject.SetActive(false);
        crosshair.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
    }

    void ShootButtonPressed()
    {
        OnUIShootButton?.Invoke();
    }
    public void UpdateScore(int score)
    {
        scoreText.text = $"SCORE: {score}";
    }
    public void UpdateTime(float time)
    {
        TimesCount.text = $"Time: {time}";
    }
    public void UpdateCoin()
    {
        Moneys.text = $"Money: {player.gold}";
    }
    public void UpdateHP()
    {
        HP.text = $"HP: {player.hp}";
    }
    public void ShopButtonPressed()
    {
        OnUIShopButton?.Invoke();
        greetingText.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        shootButton.gameObject.SetActive(false);

        crosshair.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);

        shopButton.gameObject.SetActive(false);
        //returnButton.gameObject.SetActive(false);

        settingButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
    }

    public void UpGradeButtonPressed()
    {
        OnUIShopButton?.Invoke();
    }
}
