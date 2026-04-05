using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public string plantName;
    [TextArea(3, 5)]
    public string description;
    public GameObject plantPrefab;
    public int amount;
    public Sprite icon;
    public bool isDiscovered;
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

    // --- NEW BOTANY BOOK LOGIC ---
    public void AddItem(string plantName, int amount)
    {
        InventoryItem item = items.Find(i => i.plantName == plantName);
        if (item != null)
        {
            item.amount += amount;
            item.isDiscovered = true;
        }
    }

    // --- ADDED BACK FOR TEAMMATES (Fixes all 10 errors) ---

    // Fixes "No overload for AddItem takes 4 arguments"
    public void AddItem(string plantName, GameObject prefab, Sprite icon, int amount)
    {
        AddItem(plantName, amount); // Just redirects to the new version
    }

    // Fixes "Does not contain a definition for HasItem"
    public bool HasItem(string plantName)
    {
        InventoryItem item = items.Find(i => i.plantName == plantName);
        return item != null && item.amount > 0;
    }

    // Fixes "Does not contain a definition for IncItem"
    public void IncItem(string plantName)
    {
        AddItem(plantName, 1);
    }

    // Fixes "Does not contain a definition for UseItem"
    public void UseItem(string plantName)
    {
        InventoryItem item = items.Find(i => i.plantName == plantName);
        if (item != null && item.amount > 0)
        {
            item.amount--;
        }
    }
}