using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int initialSize = 10;

    private List<GameObject> inactiveObjects = new List<GameObject>();

    public void Initialize(GameObject prefabToUse, int size)
    {
        if (prefabToUse == null)
        {
            Debug.LogError("[ObjectPool] Initialize failed: prefab is null.");
            return;
        }

        prefab = prefabToUse;
        initialSize = Mathf.Max(0, size);

        inactiveObjects.Clear();

        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            inactiveObjects.Add(obj);
        }
    }

    public GameObject GetObject()
    {
        if (prefab == null)
        {
            Debug.LogError("[ObjectPool] GetObject failed: prefab is null.");
            return null;
        }

        if (inactiveObjects.Count > 0)
        {
            GameObject pooled = inactiveObjects[0];
            inactiveObjects.RemoveAt(0);

            if (pooled != null)
            {
                pooled.SetActive(true);
                Debug.Log("[ObjectPool] Reusing pooled object.");
                return pooled;
            }
        }

        GameObject created = Instantiate(prefab, transform);
        Debug.Log("[ObjectPool] Instantiated new object.");
        return created;
    }

    public void ReturnObject(GameObject obj)
    {
        if (obj == null) return;

        // Prevent duplicate adds to pool list
        if (inactiveObjects.Contains(obj)) return;

        obj.SetActive(false);
        inactiveObjects.Add(obj);
    }
}