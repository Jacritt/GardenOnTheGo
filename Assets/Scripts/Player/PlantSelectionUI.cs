using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlantSelectionUI : MonoBehaviour
{
    public static PlantSelectionUI Instance;

    public GameObject panel;
    public Transform buttonContainer;
    public GameObject buttonTemplate;

    private PlotClick activePlot; // The plot player clicked

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void Open(PlotClick plot)
    {
        activePlot = plot;
        panel.SetActive(true);

        // Clear previous buttons
        foreach (Transform child in buttonContainer)
            Destroy(child.gameObject);

        // Create buttons based on inventory
        foreach (var item in PlayerInventory.Instance.items)
        {
            if (item.amount > 0)
            {
                GameObject btn = Instantiate(buttonTemplate, buttonContainer);
                btn.SetActive(true);

                btn.GetComponentInChildren<TMP_Text>().text = item.plantName + " (" + item.amount + ")";

                btn.GetComponent<Button>().onClick.AddListener(() =>
                {
                    activePlot.Plant(item.plantPrefab, item.plantName);
                    PlayerInventory.Instance.UseItem(item.plantName);
                    Close();
                });
            }
        }
    }

    public void Close()
    {
        panel.SetActive(false);
    }
}

