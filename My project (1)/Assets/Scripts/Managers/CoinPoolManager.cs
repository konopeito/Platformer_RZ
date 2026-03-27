using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPoolManager : MonoBehaviour
{
    public static CoinPoolManager Instance { get; private set; }

    [Header("Pool Settings")]
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private int initialSize = 10;

    [Header("Coin Spawn Points")]
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();

    private ObjectPool coinPool;
    private List<GameObject> activeCoins = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (coinPrefab == null)
        {
            Debug.LogError("[CoinPoolManager] coinPrefab is not assigned.");
            return;
        }

        if (spawnPoints == null || spawnPoints.Count == 0)
        {
            Debug.LogWarning("[CoinPoolManager] No spawn points assigned.");
        }

        coinPool = gameObject.AddComponent<ObjectPool>();
        coinPool.Initialize(coinPrefab, initialSize);

        SpawnCoinsFromPoints();
    }

    private void SpawnCoinsFromPoints()
    {
        if (coinPool == null)
        {
            Debug.LogError("[CoinPoolManager] coinPool is null. Cannot spawn coins.");
            return;
        }

        activeCoins.Clear();

        for (int i = 0; i < spawnPoints.Count; i++)
        {
            if (spawnPoints[i] == null) continue;

            GameObject coin = coinPool.GetObject();
            if (coin == null) continue;

            coin.transform.position = spawnPoints[i].position;
            coin.transform.rotation = Quaternion.identity;

            // Ensure active state in case pool implementation changes
            if (!coin.activeSelf) coin.SetActive(true);

            activeCoins.Add(coin);
        }
    }

    public void CollectCoin(GameObject coin)
    {
        if (coin == null) return;
        if (coinPool == null) return;

        if (activeCoins.Contains(coin))
        {
            activeCoins.Remove(coin);
        }

        coinPool.ReturnObject(coin);
    }

    public void ResetCoins()
    {
        if (coinPool == null)
        {
            Debug.LogWarning("[CoinPoolManager] ResetCoins called before pool initialized.");
            return;
        }

        // Return currently active coins to pool
        for (int i = activeCoins.Count - 1; i >= 0; i--)
        {
            if (activeCoins[i] != null)
            {
                coinPool.ReturnObject(activeCoins[i]);
            }
        }

        activeCoins.Clear();

        // Respawn from original spawn points
        SpawnCoinsFromPoints();

        Debug.Log("[CoinPoolManager] Coins reset and reused.");
    }
}