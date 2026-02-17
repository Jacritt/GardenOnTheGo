using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Closing : MonoBehaviour
{
    public GameObject inventoryPanel;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        if(!inventoryPanel.activeSelf)
        {
            inventoryPanel.SetActive(true);
        }
        else
        {
            inventoryPanel.SetActive(false);
        }
    }
    

    }

