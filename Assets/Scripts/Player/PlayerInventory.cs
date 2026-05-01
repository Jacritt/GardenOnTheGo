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
    public bool isDiscovered; // This will now ONLY be flipped by the Harvest function

    [Header("Visuals")]
    public Sprite[] growthStageSprites; // Array of 3 sprites (Seed, Sprout, Mature)
    public int maxStageReached = -1;    // -1 = Not found, 0 = Seed, 1 = Sprout, 2 = Mature
    public bool isCompleted;
}

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;
    public List<InventoryItem> items = new List<InventoryItem>();
    public int playerMoney = 100;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --- UPDATED LOGIC ---
    // This adds the item to your count but DOES NOT reveal it in the book
    public void AddItem(string plantName, int amount)
    {
        InventoryItem item = items.Find(i => i.plantName == plantName);
        if (item != null)
        {
            item.amount += amount;
            // Removed isDiscovered from here so planting doesn't trigger the book!
        }
    }

    // --- NEW HARVEST LOGIC ---
    // CALL THIS FUNCTION from your Harvest script/button to reveal the plant in the book
    public void DiscoverPlant(string plantName)
    {
        InventoryItem item = items.Find(i => i.plantName == plantName);
        if (item != null)
        {
            item.isDiscovered = true;

            // Also sets it to 'Mature' stage since it was harvested
            if (item.maxStageReached < 2) item.maxStageReached = 2;

            item.isCompleted = true; // Adds the stamp
            Debug.Log(plantName + " has been officially discovered and harvested!");
        }
    }

    // --- LEGACY SUPPORT FOR TEAMMATES ---

    // public void AddItem(string plantName, GameObject prefab, Sprite icon, int amount)
    // {
    //     AddItem(plantName, amount);
    // }

    public bool HasItem(string plantName)
    {
        InventoryItem item = items.Find(i => i.plantName == plantName);
        return item != null && item.amount > 0;
    }

    public void IncItem(string plantName)
    {
        AddItem(plantName, 1);
    }

    public void UseItem(string plantName)
    {
        InventoryItem item = items.Find(i => i.plantName == plantName);
        if (item != null && item.amount > 0)
        {
            item.amount--;
        }
    }

    public void incCurrency(int amount)
    {
        playerMoney += amount;
    }

    public void deductCurrency(int amount)
    {
        playerMoney -= amount;
    }

    public int getCurrency()
    {
        return playerMoney;
    }

    public InventoryItem GetItem(string plantName)
    {
        return items.Find(i => i.plantName == plantName);
    }

    public void AddMoney(int amount)
    {
        playerMoney += amount;
    }

    public void SpendMoney(int amount)
    {
        playerMoney -= amount;
    }
}

