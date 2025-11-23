using UnityEngine;

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
        DontDestroyOnLoad(plantedInstance);

        var plant = plantedInstance.GetComponent<Plant>();
        PlantContext.selectedPlant = plant;
        PlantGameManager.Instance.selectedPlant = plant;
    }

    private void OpenPlantDetail()
    {
        Plant selectedPlant = plantedInstance.GetComponent<Plant>();
        PlantContext.selectedPlant = selectedPlant;
        PlantGameManager.Instance.selectedPlant = selectedPlant;

        UnityEngine.SceneManagement.SceneManager.LoadScene("PlantDetailScene");
    }
}


