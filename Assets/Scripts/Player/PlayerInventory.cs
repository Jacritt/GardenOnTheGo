using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public string plantName;
    public GameObject plantPrefab;
    public int amount;
}

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    public List<InventoryItem> items = new List<InventoryItem>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public InventoryItem GetItem(string plantName)
    {
        return items.Find(i => i.plantName == plantName);
    }

    public bool HasItem(string plantName)
    {
        InventoryItem item = GetItem(plantName);
        return item != null && item.amount > 0;
    }

    public void UseItem(string plantName)
    {
        InventoryItem item = GetItem(plantName);
        if (item != null && item.amount > 0)
            item.amount--;
    }
}

