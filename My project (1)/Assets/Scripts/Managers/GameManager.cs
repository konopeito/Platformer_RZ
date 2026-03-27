using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Starting Values")]
    [SerializeField] private int startingHealth = 100;
    [SerializeField] private int startingScore = 0;

    public int CurrentHealth { get; private set; }
    public int CurrentScore { get; private set; }

    // Delegates / Events
    public event Action<int> OnScoreChanged;
    public event Action<int> OnHealthChanged;
    public event Action OnGameOver;

    private bool isGameOverTriggered = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        ResetGame();
    }

    public void ResetGame()
    {
        CurrentHealth = startingHealth;
        CurrentScore = startingScore;
        isGameOverTriggered = false;

        OnHealthChanged?.Invoke(CurrentHealth);
        OnScoreChanged?.Invoke(CurrentScore);
    }

    public void AddScore(int amount)
    {
        if (isGameOverTriggered) return;
        if (amount <= 0) return;

        CurrentScore += amount;
        OnScoreChanged?.Invoke(CurrentScore);
    }

    public void TakeDamage(int amount)
    {
        if (isGameOverTriggered) return;
        if (amount <= 0) return;

        CurrentHealth -= amount;
        if (CurrentHealth < 0) CurrentHealth = 0;

        OnHealthChanged?.Invoke(CurrentHealth);

        if (CurrentHealth <= 0)
        {
            TriggerGameOver();
        }
    }

    public void TriggerGameOver()
    {
        if (isGameOverTriggered) return;

        isGameOverTriggered = true;
        OnGameOver?.Invoke();
    }
}