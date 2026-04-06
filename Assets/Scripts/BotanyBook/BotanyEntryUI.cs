using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BotanyEntryUI : MonoBehaviour
{
    public string plantToDisplay; // Type "Banana Tree" exactly here in Inspector

    [Header("UI References")]
    public Image plantDisplayImage;
    public Image stampImage;
    public TextMeshProUGUI nameText;

    private int viewingStage = 0;
    private InventoryItem data;

    public void RefreshEntry()
    {
        data = PlayerInventory.Instance.items.Find(i => i.plantName == plantToDisplay);
        if (data == null) return;

        if (data.isDiscovered)
        {
            nameText.text = data.plantName;
            viewingStage = data.maxStageReached;
            UpdateImageVisuals();
            stampImage.gameObject.SetActive(data.isCompleted);
        }
        else
        {
            nameText.text = "???";
            plantDisplayImage.sprite = data.growthStageSprites[0]; // Show seed
            plantDisplayImage.color = Color.black; // Silhouette
            stampImage.gameObject.SetActive(false);
        }
    }

    // Call this when the player clicks the plant picture
    public void CycleStages()
    {
        if (!data.isDiscovered || data.maxStageReached <= 0) return;

        viewingStage++;
        if (viewingStage > data.maxStageReached) viewingStage = 0;

        UpdateImageVisuals();
    }

    void UpdateImageVisuals()
    {
        plantDisplayImage.sprite = data.growthStageSprites[viewingStage];
        plantDisplayImage.color = Color.white;
    }
}