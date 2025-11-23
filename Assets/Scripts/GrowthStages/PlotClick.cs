using UnityEngine;
using System;

public class PlotClick : MonoBehaviour
{
    private GameObject plantedInstance;

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
    }

    private void OpenPlantDetail()
    {
        Plant selectedPlant = plantedInstance.GetComponent<Plant>();
        PlantContext.selectedPlant = selectedPlant;
        PlantGameManager.Instance.selectedPlant = selectedPlant;

        UnityEngine.SceneManagement.SceneManager.LoadScene("PlantDetailScene");
    }
}


