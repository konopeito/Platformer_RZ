using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip coinSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip backgroundMusic;

    private int previousScore;
    private int previousHealth;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PlayMusic(backgroundMusic);

        if (GameManager.Instance != null)
        {
            previousScore = GameManager.Instance.CurrentScore;
            previousHealth = GameManager.Instance.CurrentHealth;
        }
    }

    private void Update()
    {
        // Handles case where GameManager loads after AudioManager in scene transitions.
        if (GameManager.Instance != null && !isSubscribed)
        {
            SubscribeToGameManager();
        }
    }

    private bool isSubscribed = false;

    private void SubscribeToGameManager()
    {
        if (GameManager.Instance == null) return;

        GameManager.Instance.OnScoreChanged += HandleScoreChanged;
        GameManager.Instance.OnHealthChanged += HandleHealthChanged;
        isSubscribed = true;
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null && isSubscribed)
        {
            GameManager.Instance.OnScoreChanged -= HandleScoreChanged;
            GameManager.Instance.OnHealthChanged -= HandleHealthChanged;
        }
        isSubscribed = false;
    }

    private void HandleScoreChanged(int newScore)
    {
        if (newScore > previousScore)
        {
            PlaySoundEffect(coinSound);
        }

        previousScore = newScore;
    }

    private void HandleHealthChanged(int newHealth)
    {
        if (newHealth < previousHealth)
        {
            PlaySoundEffect(damageSound);
        }

        previousHealth = newHealth;
    }

    public void PlayJumpSound()
    {
        PlaySoundEffect(jumpSound);
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        if (sfxSource == null || clip == null) return;
        sfxSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource == null || clip == null) return;

        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }
}