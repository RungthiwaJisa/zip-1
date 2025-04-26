using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SocialPlatforms.Impl;


public class UIManager : MonoBehaviour
{
    [Header("Button Setup")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button shootButton;
   

    [Header("UI Setup")]
    [SerializeField] private TMP_Text greetingText;
    //week11
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private Image crosshair;

    public static event Action OnUIStartButton;
    public static event Action OnUIRestartButton;
    public static event Action OnUIShootButton;


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
    }

    void StartButtonPressed()
    {
        OnUIStartButton?.Invoke();
        greetingText.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(true);
        shootButton.gameObject.SetActive(true);

        crosshair.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        scoreText.text = $"SCORE: 0";

    }

    void RestartButtonPressed()
    {
        OnUIRestartButton?.Invoke();
        greetingText.gameObject.SetActive(true);
        startButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(false);
        shootButton.gameObject.SetActive(false);

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
}
