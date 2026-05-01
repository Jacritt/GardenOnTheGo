using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserBuyTransaction : MonoBehaviour
{
    [Header("Buttons")]
    public Button buy1;
    public Button buy2;
    public Button buy3;

    [Header("Plant Names")]
    public TMP_Text item1Name;
    public TMP_Text item2Name;
    public TMP_Text item3Name;

    [Header("Stock Text")]
    public TMP_Text item1StockTxt;
    public TMP_Text item2StockTxt;
    public TMP_Text item3StockTxt;

    [Header("Plant Images")]
    public Image item1Image;
    public Image item2Image;
    public Image item3Image;

    [Header("Money")]
    public TMP_Text stockTxt;

    private InventoryItem slot1;
    private InventoryItem slot2;
    private InventoryItem slot3;

    private int stock1;
    private int stock2;
    private int stock3;

    private int price = 10;

    void Start()
    {
        GenerateShop();

        buy1.onClick.AddListener(() => Buy(1));
        buy2.onClick.AddListener(() => Buy(2));
        buy3.onClick.AddListener(() => Buy(3));
    }

    void GenerateShop()
    {
        List<InventoryItem> pool = new List<InventoryItem>(PlayerInventory.Instance.items);

        Shuffle(pool);

        slot1 = pool[0];
        slot2 = pool[1];
        slot3 = pool[2];

        stock1 = Random.Range(1, 6);
        stock2 = Random.Range(1, 6);
        stock3 = Random.Range(1, 6);

        RefreshUI();
    }

    void RefreshUI()
    {
        item1Name.text = slot1.plantName.ToUpper();
        item2Name.text = slot2.plantName.ToUpper();
        item3Name.text = slot3.plantName.ToUpper();

        item1StockTxt.text = "IN STOCK: " + stock1;
        item2StockTxt.text = "IN STOCK: " + stock2;
        item3StockTxt.text = "IN STOCK: " + stock3;

        item1Image.sprite = slot1.icon;
        item2Image.sprite = slot2.icon;
        item3Image.sprite = slot3.icon;

        stockTxt.text = "AVAILABLE MONEY: $" + PlayerInventory.Instance.getCurrency();
    }

    void Buy(int slot)
    {
        InventoryItem chosen = null;

        switch (slot)
        {
            case 1:
                if (stock1 <= 0) return;
                chosen = slot1;
                stock1--;
                break;

            case 2:
                if (stock2 <= 0) return;
                chosen = slot2;
                stock2--;
                break;

            case 3:
                if (stock3 <= 0) return;
                chosen = slot3;
                stock3--;
                break;
        }

        if (PlayerInventory.Instance.getCurrency() < price)
        {
            Debug.Log("Not enough money.");
            return;
        }

        PlayerInventory.Instance.AddItem(chosen.plantName, 1);
        PlayerInventory.Instance.deductCurrency(price);

        RefreshUI();

        Debug.Log("Bought: " + chosen.plantName);
    }

    void Shuffle(List<InventoryItem> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);

            InventoryItem temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}