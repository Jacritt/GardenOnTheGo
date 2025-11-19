using UnityEngine;
using UnityEngine.UI;

public class PlantStageController : MonoBehaviour
{
    public Image plantImage;     // UI Image showing the plant
    public Text descriptionText; // UI Text for description
    public Text waterText;       // UI Text for water
    public Text daysText;        // UI Text for days passed
    public Text minigamesText;   // UI Text for minigames
    public Button upgradeButton; // Upgrade button

    void Start()
    {
        UpdateUI();
        UpdateUpgradeButton();
    }

    void Update()
    {
        // Continuously toggle the upgrade button
        UpdateUpgradeButton();
    }

    void UpdateUI()
    {
        // Update all UI fields from PlantContext
        plantImage.sprite = PlantContext.stageSprite;
        descriptionText.text = PlantContext.stageDescription;

        waterText.text = "Water: " + PlantContext.currentWater + "/" + PlantContext.waterRequired;
        daysText.text = "Days Passed: " + PlantContext.currentDaysPast + "/" + PlantContext.daysRequired;
        minigamesText.text = "Minigames Played: " + PlantContext.currentMinigamesPlayed + "/" + PlantContext.minigamesRequired;
    }

    void UpdateUpgradeButton()
    {
        // Show button only if all requirements are met
        bool requirementsMet = PlantContext.currentWater >= PlantContext.waterRequired &&
                               PlantContext.currentDaysPast >= PlantContext.daysRequired &&
                               PlantContext.currentMinigamesPlayed >= PlantContext.minigamesRequired;

        upgradeButton.gameObject.SetActive(requirementsMet);
    }

    public void UpgradeStage()
    {
        // TODO: You need to set the next stage values in PlantContext here
        // For example, assign:
        // PlantContext.stageSprite = nextStageSprite;
        // PlantContext.stageDescription = nextStageDescription;
        // PlantContext.waterRequired = nextStageWaterRequirement;
    }
}

