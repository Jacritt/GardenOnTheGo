using UnityEngine;

public class PlotClick : MonoBehaviour
{
    public GameObject plantPrefab;  // assign BeetrootPrefab here in inspector
    private GameObject plantedInstance;

    public void OnClick()
    {
        // Spawn an instance at the plot position
        plantedInstance = Instantiate(plantPrefab, transform.position, Quaternion.identity);
        DontDestroyOnLoad(plantedInstance);


        // Assign the Plant component to your manager/context
        Plant selectedPlant = plantedInstance.GetComponent<Plant>();
        PlantContext.selectedPlant = selectedPlant;
        PlantGameManager.Instance.selectedPlant = selectedPlant;

        Debug.Log("Selected plant instance: " + selectedPlant.plantName);

        // Load the detail scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("PlantDetailScene");
    }
}

