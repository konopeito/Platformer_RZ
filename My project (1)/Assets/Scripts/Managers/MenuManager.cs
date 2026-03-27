using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string gameSceneName = "GameScene";

    // Called by StartButton -> OnClick()
    public void StartGame()
    {
        // Reset core game state before entering gameplay
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetGame();
        }
        else
        {
            Debug.LogWarning("[MenuManager] GameManager.Instance is null. Did you add GameManager to MainMenu?");
        }

     
        //if (CoinPoolManager.Instance != null)
            //{
            //    CoinPoolManager.Instance.ResetCoins();
            //}

        if (string.IsNullOrEmpty(gameSceneName))
        {
            Debug.LogError("[MenuManager] gameSceneName is empty. Set it in Inspector.");
            return;
        }

        SceneManager.LoadScene(gameSceneName);
    }
    public void QuitGame()
    {
        Debug.Log("[MenuManager] QuitGame called.");
        Application.Quit();
    }
}