using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SocialPlatforms.Impl;


public class UIManager : MonoBehaviour
{
    public Player player;

    [Header("Button Setup")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button shootButton;
    [SerializeField] private Button shopButton;
    //[SerializeField] private Button returnButton;

    [Header("UI Setup")]
    //[SerializeField] private TMP_Text greetingText;
    //week11
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text TimesCount;
    [SerializeField] private Image crosshair;

    public static event Action OnUIStartButton;
    public static event Action OnUIRestartButton;
    public static event Action OnUIShootButton;
    public static event Action OnUIShopButton;
    public static event Action OnUIUpGradeButton;
    public static event Action OnUIReturnButton;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startButton.onClick.AddListener(StartButtonPressed);
        restartButton.onClick.AddListener(RestartButtonPressed);
        shootButton.onClick.AddListener(ShootButtonPressed);

        restartButton.gameObject.SetActive(false);
        shootButton.gameObject.SetActive(false);
        //week11
        crosshair.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);

        //shopButton.gameObject.SetActive(false);
        //upgradeButton.gameObject.SetActive(false);
        //returnButton.gameObject.SetActive(false);
    }

    void StartButtonPressed()
    {
        OnUIStartButton?.Invoke();
        //greetingText.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(true);
        shootButton.gameObject.SetActive(true);

        crosshair.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        scoreText.text = $"SCORE: 0";

        //shopButton.gameObject.SetActive(false);
        //returnButton.gameObject.SetActive(false);
    }

    void RestartButtonPressed()
    {
        OnUIRestartButton?.Invoke();
        //greetingText.gameObject.SetActive(true);
        startButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(false);
        shootButton.gameObject.SetActive(false);

        crosshair.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);

        //shopButton.gameObject.SetActive(false);
        //returnButton.gameObject.SetActive(false);
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
    public void ShopButtonPressed()
    {
        OnUIShopButton?.Invoke();
        //greetingText.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        shootButton.gameObject.SetActive(false);

        crosshair.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);

        //shopButton.gameObject.SetActive(false);
        //returnButton.gameObject.SetActive(false);


    }

    public void UpGradeButtonPressed()
    {
        OnUIShopButton?.Invoke();
    }

    public void ReturnButtonPressed()
    {
        OnUIReturnButton?.Invoke();
        //greetingText.gameObject.SetActive(true);
        startButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(false);
        shootButton.gameObject.SetActive(false);

        crosshair.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);

        //shopButton.gameObject.SetActive(false);
        //returnButton.gameObject.SetActive(false);
    }
}
