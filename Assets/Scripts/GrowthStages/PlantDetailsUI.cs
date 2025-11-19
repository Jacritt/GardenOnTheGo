using UnityEngine;
using UnityEngine.UI;

public class PlantDetailsUI : MonoBehaviour
{
    public Image plantImage;
    public Text descriptionText;
    public Text waterText;
    public Text daysText;
    public Text minigamesText;

    void Start()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        plantImage.sprite = PlantContext.stageSprite;
        descriptionText.text = PlantContext.stageDescription;

        waterText.text = "Water: " + PlantContext.currentWater + "/" + PlantContext.waterRequired;
        daysText.text = "Days Passed: " + PlantContext.currentDaysPast + "/" + PlantContext.daysRequired;
        minigamesText.text = "Minigames Played: " + PlantContext.currentMinigamesPlayed + "/" + PlantContext.minigamesRequired;
    }
}

