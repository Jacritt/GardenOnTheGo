using UnityEngine;
using UnityEngine.UI;
using System;

public class PlotClick : MonoBehaviour
{
    public Image plotButtonImage;
    public Image plantImage;
    public TMPro.TextMeshProUGUI plotButtonText;

    public string plotId;

    private GameObject plantedInstance;

    private void UpdateVisualState(bool hasPlant)
    {
        if (plotButtonImage != null)
            plotButtonImage.enabled = !hasPlant; // Show background only when no plant

        if (plantImage != null)
            plantImage.enabled = hasPlant; // Show plant image only when planted

        if (plotButtonText != null)
            plotButtonText.enabled = !hasPlant;
    }

    public void OnClick()
    {
        if (plantedInstance == null)
        {
            // Open UI for selecting plant
            PlantSelectionUI.Instance.Open(this);
        }
        else
        {
            // Already planted ? open detail
            OpenPlantDetail();
        }
    }

    // Called by the UI selection buttons
    public void Plant(GameObject prefab, string plantName)
    {
        plantedInstance = Instantiate(prefab, transform.position, Quaternion.identity);

        Plant plant = plantedInstance.GetComponent<Plant>();

        // IMPORTANT: ensure unique Save ID exists
        if (string.IsNullOrEmpty(plant.UniqueId))
            plant.UniqueId = Guid.NewGuid().ToString();

        DontDestroyOnLoad(plantedInstance);

        PlantGameManager.Instance.selectedPlant = plant;
        PlantContext.selectedPlant = plant;

        // ADD THE PLANT TO PlantGameManager’s list and dictionary
        PlantGameManager.Instance.RegisterPlant(plant);

        PlantGameManager.Instance.plantsByPlot[plotId] = plant;
        // Update the plot's button UI to match the plant stage
        var stage = plant.GetStage();
        if (plantImage != null && stage != null)
            plantImage.sprite = stage.sprite;

        UpdateVisualState(true);
    }

    private void OnEnable()
    {
        if (PlantGameManager.Instance != null &&
            PlantGameManager.Instance.plantsByPlot.TryGetValue(plotId, out Plant plant))
        {
            plantedInstance = plant.gameObject;

            // Update plant sprite
            if (plantImage != null)
                plantImage.sprite = plant.GetStage().sprite;

            // Update button/text/image visibility
            UpdateVisualState(true);
        }
        else
        {
            UpdateVisualState(false);
        }
    }

    private void OpenPlantDetail()
    {
        Plant selectedPlant = plantedInstance.GetComponent<Plant>();
        PlantContext.selectedPlant = selectedPlant;
        PlantGameManager.Instance.selectedPlant = selectedPlant;

        UnityEngine.SceneManagement.SceneManager.LoadScene("PlantDetailScene");
    }
}


