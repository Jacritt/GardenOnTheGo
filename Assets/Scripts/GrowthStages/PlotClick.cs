using UnityEngine;
using UnityEngine.UI;
using System;

public class PlotClick : MonoBehaviour
{
    [Header("UI")]
    public Image plotButtonImage;
    public Image[] plantImages;
    public TMPro.TextMeshProUGUI plotButtonText;

    public string plotId;

    private GameObject plantedInstance;

    private void UpdateVisualState(bool hasPlant)
    {
        if (plotButtonImage != null)
            plotButtonImage.enabled = !hasPlant; // Show background only when no plant

        if (plantImages != null)
        {
            foreach (Image img in plantImages)
            {
                if (img != null)
                    img.enabled = hasPlant;
            }
        } // Show plant image only when planted

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
        DontDestroyOnLoad(plantedInstance);

        Plant plant = plantedInstance.GetComponent<Plant>();
        plant.currentStage = 0;
        plant.currentWater = 0;
        plant.currentDays = 0;
        plant.currentMinigames = 0;

        plant.prefabSource = prefab;
        Debug.Log($"[PlotClick] Planting {plantName} with prefab {prefab.name}");
        Debug.Log("Planted plant with ID: " + plant.UniqueId);

        // IMPORTANT: ensure unique Save ID exists
        if (string.IsNullOrEmpty(plant.UniqueId))
            plant.UniqueId = System.Guid.NewGuid().ToString();

        plant.plotId = plotId;

        PlantGameManager.Instance.SetSelectedPlant(plant);
        PlantContext.selectedPlant = plant;

        // ADD THE PLANT TO PlantGameManager’s list and dictionary
        PlantGameManager.Instance.RegisterPlant(plant);

        PlantGameManager.Instance.plantsByPlot[plotId] = plant;
        // Update the plot's button UI to match the plant stage
        var stage = plant.GetStage();
        if (plantImages != null && stage != null)
        {
            foreach (Image img in plantImages)
            {
                if (img != null)
                    img.sprite = stage.sprite;
            }
        }

        UpdateVisualState(true);
        Debug.Log($"[PlotClick] Plant {plantName} registered and UI updated for plotId {plotId}");
        plant.OnProgressChanged += HandlePlantChanged;
    }

    private void HandlePlantChanged(Plant plant)
    {
        var sprite = plant.GetStage()?.sprite;

        if (plantImages != null)
        {
            foreach (Image img in plantImages)
            {
                if (img != null)
                    img.sprite = sprite;
            }
        }
    }

    private void OnEnable()
    {
        if (PlantGameManager.Instance.plantsByPlot.TryGetValue(plotId, out Plant plant))
        {
            plantedInstance = plant.gameObject;

            plant.OnProgressChanged -= HandlePlantChanged;
            plant.OnProgressChanged += HandlePlantChanged;

            var sprite = plant.GetStage()?.sprite;

            if (plantImages != null)
            {
                foreach (Image img in plantImages)
                {
                    if (img != null)
                        img.sprite = sprite;
                }
            }

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

        //PlantSelectionUI.Instance.HideCanvas();
        UnityEngine.SceneManagement.SceneManager.LoadScene("PlantDetailScene");

    }

    public void RefreshFromManager()
    {
        if (PlantGameManager.Instance.plantsByPlot.TryGetValue(plotId, out Plant plant))
        {
            plantedInstance = plant.gameObject;

            var sprite = plant.GetStage()?.sprite;

            if (plantImages != null)
            {
                foreach (Image img in plantImages)
                {
                    if (img != null)
                        img.sprite = sprite;
                }
            }

            UpdateVisualState(true);
        }
    }

}


