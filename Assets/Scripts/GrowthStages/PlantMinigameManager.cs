using UnityEngine;

public class PlantMinigameManager : MonoBehaviour
{
    public static PlantMinigameManager Instance;
    public GameObject minigamePanel;
    public Plant currentPlant;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Open(Plant plant)
    {
        currentPlant = plant;
        if (minigamePanel) minigamePanel.SetActive(true);
        Debug.Log("Minigame opened for plant " + plant.GetPlantId());
    }

    public void OnSuccess()
    {
        if (currentPlant)
        {
            currentPlant.NotifyMinigameCompleted();
            Debug.Log("Minigame completed!");
        }
        if (minigamePanel) minigamePanel.SetActive(false);
    }

    public void SetPlant(Plant plant)
    {
        currentPlant = plant;
    }

    // Call this when the player completes the minigame successfully
    public void CompleteMinigame()
    {
        if (currentPlant != null)
        {
            currentPlant.NotifyMinigameCompleted();
            Debug.Log($"✅ Minigame completed for {currentPlant.GetPlantId()}");
        }
        else
        {
            Debug.LogWarning("No plant assigned to minigame manager!");
        }

        if (minigamePanel != null)
            minigamePanel.SetActive(false);
        else
            gameObject.SetActive(false);

    }
    public void OnMinigameWin()
    {
        CompleteMinigame(); // ✅ Closes and updates plant
    }

}

