using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserBuyTransaction : MonoBehaviour
{
    public static UserBuyTransaction Instance;
    public Button buy1, buy2, buy3;
    public TMP_Text item1StockTxt, item2StockTxt, item3StockTxt;
    public GameObject prefab1, prefab2, prefab3;
    public Sprite icon1, icon2, icon3;

    // Start is called before the first frame update
    void Start()
    {
        buy1.onClick.AddListener(delegate {decrementStock("Btn1"); });
        buy2.onClick.AddListener(delegate {decrementStock("Btn2"); });
        buy3.onClick.AddListener(delegate {decrementStock("Btn3"); });
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Trigger via BUY button click
    public void decrementStock(string buySelect)
    {
        if(buySelect == "Btn1")
        { 
            Debug.Log("BUY 1 CLICKED");
            string oldStock = item1StockTxt.text;
            int length = oldStock.Length;
            
            int val = oldStock[length-1] - '0';
            Debug.Log(val);
            
            if(val <= 0)
                item1StockTxt.text = "Out of Stock!";
            else
            {
                if(PlayerInventory.Instance.HasItem("Red Pepper"))
                    PlayerInventory.Instance.IncItem("Red Pepper");
                else
                    PlayerInventory.Instance.AddItem("Red Pepper", prefab1, icon1, 1);
            }

            if(val > 0)
            {
                val -= 1;
                if(val == 0)
                    item1StockTxt.text = "Out of Stock!";
                else
                    item1StockTxt.text = oldStock.Substring(0, length-1) + "" + val;
            }
        }

        if(buySelect == "Btn2")
        { 
            Debug.Log("BUY 2 CLICKED");
            string oldStock = item2StockTxt.text;
            int length = oldStock.Length;
            
            int val = oldStock[length-1] - '0';
            Debug.Log(val);

            if(val <= 0)
                item2StockTxt.text = "Out of Stock!";
            else
            {
                if(PlayerInventory.Instance.HasItem("Potato"))
                    PlayerInventory.Instance.IncItem("Potato");
                else
                    PlayerInventory.Instance.AddItem("Potato", prefab2, icon2, 1);
            }

            if(val > 0)
            {
                val -= 1;
                if(val == 0)
                    item2StockTxt.text = "Out of Stock!";
                else
                    item2StockTxt.text = oldStock.Substring(0, length-1) + "" + val;
            }
        }

        if(buySelect == "Btn3")
        {
            Debug.Log("BUY 3 CLICKED");
            string oldStock = item3StockTxt.text;
            int length = oldStock.Length;
            
            int val = oldStock[length-1] - '0';
            Debug.Log(val);
            
            if(val <= 0)
                item3StockTxt.text = "Out of Stock!";
            else
            {
                if(PlayerInventory.Instance.HasItem("Lemon Tree"))
                    PlayerInventory.Instance.IncItem("Lemon Tree");
                else
                    PlayerInventory.Instance.AddItem("Lemon Tree", prefab3, icon3, 1);
            }

            if(val > 0)
            {
                val -= 1;
                if(val == 0)
                    item3StockTxt.text = "Out of Stock!";
                else
                    item3StockTxt.text = oldStock.Substring(0, length-1) + "" + val;
            }
        }
        // TODO: Reference number in stock text ("In Stock: XX")
        //       Add plant type bought to player inventory
        
        
        //       Decrease value in stock text


        reflectOutOfStock();
    }

    public void reflectOutOfStock()
    {
        // TODO: Check if stock text = 0 for plant
        //       If true, grey out sprite* (or simply declare "OUT OF STOCK" in text)
    }
}
