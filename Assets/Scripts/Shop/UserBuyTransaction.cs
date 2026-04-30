using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserBuyTransaction : MonoBehaviour
{
    public static UserBuyTransaction Instance;
    public Button buy1, buy2, buy3;
    public TMP_Text item1Name, item2Name, item3Name;
    public TMP_Text item1StockTxt, item2StockTxt, item3StockTxt;
    public int item1Stock, item2Stock, item3Stock;
    public GameObject prefab1, prefab2, prefab3;
    public GameObject item1Sprite, item2Sprite, item3Sprite;
    public TMP_Text stockTxt;
    public Sprite icon1, icon2, icon3;

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        { Instance = this; }
        // else if (Instance != this)
        // { Destroy(gameObject); }

        DontDestroyOnLoad(gameObject);
    }
    
    void Start()
    {
        // Declare shop vars
        ShopItem[] currStock = ShopInventory.Instance.reStock();

        prefab1 = currStock[0].plantPrefab; 
        prefab2 = currStock[1].plantPrefab; 
        prefab3 = currStock[2].plantPrefab;

        Debug.Log(prefab1);
        Debug.Log(prefab2);
        Debug.Log(prefab3);

        item1Stock = currStock[0].quantity; 
        item2Stock = currStock[1].quantity; 
        item3Stock = currStock[2].quantity;

        Debug.Log(item1Stock);
        Debug.Log(item1Stock);
        Debug.Log(item1Stock);

        item1Name.text = currStock[0].plantName;
        item2Name.text = currStock[1].plantName;
        item3Name.text = currStock[2].plantName;

    //    item1Sprite.sprite = currStock[0].plantPrefab.
        

        buy1.onClick.AddListener(delegate {decrementStock(currStock[0]); });
        buy2.onClick.AddListener(delegate {decrementStock(currStock[1]); });
        buy3.onClick.AddListener(delegate {decrementStock(currStock[2]); });
    }

    // Update is called once per frame
    void Update()
    {
        ShopItem[] currStock = ShopInventory.Instance.reStock();

        prefab1 = currStock[0].plantPrefab; 
        prefab2 = currStock[1].plantPrefab; 
        prefab3 = currStock[2].plantPrefab;

        item1Stock = currStock[0].quantity; 
        item2Stock = currStock[1].quantity; 
        item3Stock = currStock[2].quantity;

        item1Name.text = currStock[0].plantName.ToUpper();
        item2Name.text = currStock[1].plantName.ToUpper();
        item3Name.text = currStock[2].plantName.ToUpper();

        item1StockTxt.text = "IN STOCK: " + item1Stock;
        item2StockTxt.text = "IN STOCK: " + item2Stock;
        item3StockTxt.text = "IN STOCK: " + item3Stock;

        item1Sprite.GetComponent<SpriteRenderer>().sprite = currStock[0].plantSprite;
        item2Sprite.GetComponent<SpriteRenderer>().sprite = currStock[1].plantSprite;
        item3Sprite.GetComponent<SpriteRenderer>().sprite = currStock[2].plantSprite;

        string stockTxtSpliced = stockTxt.text;
        stockTxt.text = stockTxtSpliced.Substring(0,18) + " " + PlayerInventory.Instance.getCurrency();
    }

    // Trigger via BUY button click
    public void decrementStock(ShopItem toBuy)
    {
        
        int val = toBuy.quantity;
        
        if((val > 0) && (PlayerInventory.Instance.getCurrency() >= toBuy.cost))
        {
            if(PlayerInventory.Instance.HasItem(toBuy.plantName))
                PlayerInventory.Instance.IncItem(toBuy.plantName);
            else
                PlayerInventory.Instance.AddItem(toBuy.plantName, toBuy.plantPrefab, 1);

            PlayerInventory.Instance.deductCurrency(toBuy.cost);
            ShopInventory.Instance.decStock(toBuy); 
        }

        reflectOutOfStock();
    }

    public void reflectOutOfStock()
    {
        // TODO: Check if stock text = 0 for plant
        //       If true, grey out sprite* (or simply declare "OUT OF STOCK" in text)
    }
}
