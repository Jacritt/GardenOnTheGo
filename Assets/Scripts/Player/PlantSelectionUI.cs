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

    public void Open(PlotClick plot)
    {
        activePlot = plot;
        panel.SetActive(true);

        // Clear previous buttons
        foreach (Transform child in buttonContainer)
            Destroy(child.gameObject);

        // Create buttons based on inventory
        Debug.Log("items:");
        Debug.Log(PlayerInventory.Instance.items);
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

                if (iconTransform == null || amountTransform == null)
                {
                    Debug.LogError("Icon or AmountText missing on ButtonTemplate!");
                    continue;
                }

                Image icon = iconTransform.GetComponent<Image>();
                TMP_Text amountText = amountTransform.GetComponent<TMP_Text>();
                TMP_Text nameText = nameTransform.GetComponent<TMP_Text>();

                if (icon == null || amountText == null)
                {
                    Debug.LogError("Missing Image or TMP_Text component!");
                    continue;
                }

                icon.sprite = item.icon;
                amountText.text = item.amount.ToString();
                nameText.text = item.plantName;

                btn.GetComponent<Button>().onClick.AddListener(() =>
                {
                    activePlot.Plant(item.plantPrefab, item.plantName);
                    PlayerInventory.Instance.UseItem(item.plantName);
                    Close();
                });
            }
        }
        Debug.Log("Inventory count: " + PlayerInventory.Instance.items.Count);
    }

    public void Close()
    {
        panel.SetActive(false);
    }
}

