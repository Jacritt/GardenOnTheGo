using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlantSelectionUI : MonoBehaviour
{
    public static PlantSelectionUI Instance;
    private bool isPlantingMode = false;

    public GameObject panel;
    public Transform buttonContainer;
    public GameObject buttonTemplate;

    private PlotClick activePlot; // The plot player clicked

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            // Make sure we are a root object that can be DontDestroyOnLoad'd
            // If this object has parents, you should call DontDestroyOnLoad on the root.
            //DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        if (panel != null)
            panel.SetActive(false);

        // Ensure template is inactive by default
        if (buttonTemplate != null)
            buttonTemplate.SetActive(false);
    }

    public void Open(PlotClick plot = null)
    {
        activePlot = plot;
        isPlantingMode = (plot != null); // planting mode only if a plot is passed

        panel.SetActive(true);

        // Clear previous buttons
        foreach (Transform child in buttonContainer)
            Destroy(child.gameObject);

        PlayerInventory.Instance.items.RemoveAll(i => i.amount <= 0);

        foreach (var item in PlayerInventory.Instance.items)
        {
            if (item.amount > 0)
            {
                GameObject btn = Instantiate(buttonTemplate, buttonContainer, false);
                btn.SetActive(true);

                Transform iconTransform = btn.transform.Find("Icon");
                Transform amountTransform = btn.transform.Find("AmountText");
                Transform nameTransform = btn.transform.Find("NameText");

                Image icon = iconTransform.GetComponent<Image>();
                TMP_Text amountText = amountTransform.GetComponent<TMP_Text>();
                TMP_Text nameText = nameTransform.GetComponent<TMP_Text>();

                icon.sprite = item.icon;
                amountText.text = item.amount.ToString();
                nameText.text = item.plantName;

                Button button = btn.GetComponent<Button>();

                // ?? KEY CHANGE HERE
                button.onClick.RemoveAllListeners();

                if (isPlantingMode)
                {
                    button.onClick.AddListener(() =>
                    {
                        activePlot.Plant(item.plantPrefab, item.plantName);
                        PlayerInventory.Instance.UseItem(item.plantName);
                        Close();
                    });
                }
                else
                {
                    // View-only mode ? do nothing (or optional UI feedback)
                    button.interactable = false; // optional: visually disable
                }
            }
        }
    }

    public void Close()
    {
        panel.SetActive(false);
    }
}

