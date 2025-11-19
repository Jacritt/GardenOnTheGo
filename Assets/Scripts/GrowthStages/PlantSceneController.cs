using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlantSceneController : MonoBehaviour
{
    [Header("UI References")]
    public Image plantImage;
    public Text plantNameText;
    public Text descriptionText;
    public Text waterText;
    public Text daysText;
    public Text minigamesText;

    [Header("Optional: Back button to return to the garden")]
    public string gardenSceneName = "GardenScene";

    private void Start()
    {
        PopulateUIFromContext();
    }

    public void PopulateUIFromContext()
    {
        if (PlantContext.stageSprite != null && plantImage != null)
        {
            plantImage.sprite = PlantContext.stageSprite;
            plantImage.preserveAspect = true;
        }

        if (plantNameText != null)
            plantNameText.text = string.IsNullOrEmpty(PlantContext.plantName) ? "Plant" : PlantContext.plantName;

        if (descriptionText != null)
            descriptionText.text = PlantContext.stageDescription ?? "";

        UpdateProgressUI();
    }

    public void UpdateProgressUI()
    {
        if (waterText != null)
            waterText.text = $"Water: {PlantContext.currentWater}/{PlantContext.waterRequired}";
        if (daysText != null)
            daysText.text = $"Days Passed: { PlantContext.currentDaysPast}/{ PlantContext.daysRequired}";
        if (minigamesText != null)
            minigamesText.text = $"Mingames Played: {PlantContext.currentMinigamesPlayed}/{PlantContext.daysRequired}";
    }

    public void OnBackToGarden()
    {
        // Optionally clear context or persist changes first
        SceneManager.LoadScene(gardenSceneName);
    }

    private void OnDestroy()
    {
        // keep or clear context depending on your persistence needs:
        // PlantContext.Clear();
    }
}

