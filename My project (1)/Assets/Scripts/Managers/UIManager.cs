using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text healthText;

    [Header("Scene Names")]
    [SerializeField] private string gameOverSceneName = "GameOver";

    private bool isSubscribed = false;

    private void Start()
    {
        // Handles timing cases where GameManager may initialize after this object
        TrySubscribe();
    }

    private void OnEnable()
    {
        TrySubscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void TrySubscribe()
    {
        if (isSubscribed) return;
        if (GameManager.Instance == null) return;

        GameManager.Instance.OnScoreChanged += UpdateScoreUI;
        GameManager.Instance.OnHealthChanged += UpdateHealthUI;
        GameManager.Instance.OnGameOver += HandleGameOver;
        isSubscribed = true;

        // Immediately sync UI values
        UpdateScoreUI(GameManager.Instance.CurrentScore);
        UpdateHealthUI(GameManager.Instance.CurrentHealth);
    }

    private void Unsubscribe()
    {
        if (!isSubscribed) return;
        if (GameManager.Instance == null) return;

        GameManager.Instance.OnScoreChanged -= UpdateScoreUI;
        GameManager.Instance.OnHealthChanged -= UpdateHealthUI;
        GameManager.Instance.OnGameOver -= HandleGameOver;
        isSubscribed = false;
    }

    private void UpdateScoreUI(int newScore)
    {
        Debug.Log("[UIManager] Score changed: " + newScore);

        if (scoreText != null)
        {
            scoreText.text = "Score: " + newScore;
        }
    }

    private void UpdateHealthUI(int newHealth)
    {
        Debug.Log("[UIManager] Health changed: " + newHealth);

        if (healthText != null)
        {
            healthText.text = "Health: " + newHealth;
        }
    }

    private void HandleGameOver()
    {
        Debug.Log("[UIManager] Game over triggered.");

        if (!string.IsNullOrEmpty(gameOverSceneName))
        {
            SceneManager.LoadScene(gameOverSceneName);
        }
        else
        {
            Debug.LogError("[UIManager] gameOverSceneName is empty in Inspector.");
        }
    }
}