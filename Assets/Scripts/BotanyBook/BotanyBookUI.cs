using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BotanyBookUI : MonoBehaviour
{
    [Header("UI Panel")]
    public GameObject bookPanel;

    [Header("Display Elements")]
    public TextMeshProUGUI plantNameText;
    public TextMeshProUGUI descriptionText;
    public Image plantIcon;
    public TextMeshProUGUI statsText; // Show how many harvested

    private int currentIndex = 0;

    void Start()
    {
        // Start with the book closed
        if (bookPanel != null) bookPanel.SetActive(false);
    }

    void Update()
    {
        // Press 'B' for Botany Book!
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleBook();
        }
    }

    public void ToggleBook()
    {
        bool isActive = !bookPanel.activeSelf;
        bookPanel.SetActive(isActive);

        if (isActive)
        {
            UpdateBookDisplay();
        }
    }

    public void NextPage()
    {
        if (PlayerInventory.Instance.items.Count == 0) return;

        currentIndex = (currentIndex + 1) % PlayerInventory.Instance.items.Count;
        UpdateBookDisplay();
    }

    public void PreviousPage()
    {
        if (PlayerInventory.Instance.items.Count == 0) return;

        currentIndex--;
        if (currentIndex < 0) currentIndex = PlayerInventory.Instance.items.Count - 1;

        UpdateBookDisplay();
    }

    public void UpdateBookDisplay()
    {
        if (PlayerInventory.Instance.items.Count == 0) return;

        InventoryItem currentItem = PlayerInventory.Instance.items[currentIndex];

        if (currentItem.isDiscovered)
        {
            plantNameText.text = currentItem.plantName;
            descriptionText.text = currentItem.description;
            plantIcon.sprite = currentItem.icon;
            plantIcon.color = Color.white; // Show full color
            statsText.text = "Total Harvested: " + currentItem.amount;
        }
        else
        {
            plantNameText.text = "???";
            descriptionText.text = "This plant has not been discovered yet. Keep gardening to unlock this entry!";
            plantIcon.sprite = currentItem.icon;
            plantIcon.color = Color.black; // Show silhouette
            statsText.text = "Unknown Species";
        }
    }
}