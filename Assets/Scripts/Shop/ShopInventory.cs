using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopItem
{
    public string plantName;
    public GameObject plantPrefab;
    public Sprite plantSprite;
    public int quantity = 3;
    public int cost;
}

public class ShopInventory : MonoBehaviour
{
    public static ShopInventory Instance;
    public List<ShopItem> shopStock = new List<ShopItem>();
    public int currCycle = 0;
    public int stockSet = 0;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public ShopItem[] reStock()
    {
        stockSet = ( currCycle / 9 ) * 3;  // each time a shop plant is out of stock

        ShopItem[] prefabsToSend = {shopStock[stockSet], shopStock[stockSet + 1], shopStock[stockSet + 2]};
        return prefabsToSend;
    }

    void findStock(ShopItem purchasedItem)
    {

    }

    public void decStock(ShopItem purchasedItem)
    {
        if(purchasedItem.quantity - 1 < 0)
        { purchasedItem.quantity = 0; }
        else
        {
            purchasedItem.quantity -= 1;
            currCycle += 1;
        }
    }
}
