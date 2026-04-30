using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BotanyEntryUI : MonoBehaviour
{
    [Header("Plant Identity")]
    public string plantToDisplay;

    [Header("Visual References")]
    public GameObject silhouetteObject;
    public GameObject colorObject;
    public Image plantDisplayImage;
    public Image stampImage;

    [Header("Text References")]
    public TextMeshPro nameText;
    public GameObject descriptionObject; // Drag the 'Description' child here

    private int viewingStage = 0;
    private InventoryItem data;

    private void OnEnable()
    {
        RefreshEntry();
    }

    public void RefreshEntry()
    {
        data = PlayerInventory.Instance.items.Find(i => i.plantName == plantToDisplay);

        if (data == null) return;

        if (data.isDiscovered)
        {
            // --- DISCOVERED STATE ---
            nameText.text = data.plantName; // Shows "Banana Tree"
            if (descriptionObject != null) descriptionObject.SetActive(true); // Shows description

            silhouetteObject.SetActive(false);
            colorObject.SetActive(true);

            viewingStage = data.maxStageReached;
            UpdateImageVisuals();

            if (stampImage != null) stampImage.gameObject.SetActive(data.isCompleted);
        }
        else
        {
            // --- MYSTERY STATE ---
            nameText.text = "???"; // Hides the name
            if (descriptionObject != null) descriptionObject.SetActive(false); // Hides description

            silhouetteObject.SetActive(true);
            colorObject.SetActive(false);

            if (stampImage != null) stampImage.gameObject.SetActive(false);
        }
    }

    public void CycleStages()
    {
        if (data == null || !data.isDiscovered || data.maxStageReached <= 0) return;
        viewingStage++;
        if (viewingStage > data.maxStageReached) viewingStage = 0;
        UpdateImageVisuals();
    }

    void UpdateImageVisuals()
    {
        if (data.growthStageSprites != null && data.growthStageSprites.Length > viewingStage)
        {
            plantDisplayImage.sprite = data.growthStageSprites[viewingStage];
        }
    }
}