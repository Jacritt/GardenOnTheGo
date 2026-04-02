using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public string plantName;
    [TextArea(3, 5)] // Makes it easier to type descriptions in the Inspector
    public string description;
    public GameObject plantPrefab;
    public int amount;
    public Sprite icon;
    public bool isDiscovered; // New: Tracks if it should show in the Botany Book
}

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;
    public List<InventoryItem> items = new List<InventoryItem>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddItem(string plantName, int amount)
    {
        InventoryItem item = items.Find(i => i.plantName == plantName);

        if (item != null)
        {
            item.amount += amount;
            item.isDiscovered = true; // Mark as found
            Debug.Log($"Harvested {amount} {plantName}(s). Total: {item.amount}");
        }
        else
        {
            Debug.LogWarning($"Plant {plantName} not found in Inventory Database!");
        }
    }

    public InventoryItem GetItem(string plantName) => items.Find(i => i.plantName == plantName);
}