using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text finalScoreText;

    [Header("Scene Names")]
    [SerializeField] private string gameSceneName = "GameScene";
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private void Start()
    {
        int finalScore = 0;

        if (GameManager.Instance != null)
        {
            finalScore = GameManager.Instance.CurrentScore;
        }
        else
        {
            Debug.LogWarning("[GameOverManager] GameManager.Instance is null. Final score defaults to 0.");
        }

        if (finalScoreText != null)
        {
            finalScoreText.text = "Final Score: " + finalScore;
        }
        else
        {
            Debug.LogWarning("[GameOverManager] finalScoreText is not assigned in Inspector.");
        }
    }

    public void RetryGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetGame();
        }

        if (CoinPoolManager.Instance != null)
        {
            CoinPoolManager.Instance.ResetCoins();
        }

        if (string.IsNullOrEmpty(gameSceneName))
        {
            Debug.LogError("[GameOverManager] gameSceneName is empty in Inspector.");
            return;
        }

        SceneManager.LoadScene(gameSceneName);
    }

    public void BackToMainMenu()
    {
        if (string.IsNullOrEmpty(mainMenuSceneName))
        {
            Debug.LogError("[GameOverManager] mainMenuSceneName is empty in Inspector.");
            return;
        }

        SceneManager.LoadScene(mainMenuSceneName);
    }
}