// PlantSceneController.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlantSceneController : MonoBehaviour
{
    [Header("UI references")]
    public Image plantImage;
    public Text plantNameText;
    public Text descriptionText;
    public Text waterText;
    public Text daysText;
    public Text minigamesText;
    public Button upgradeButton;
    public Button harvestButton;

    [Header("Return")]
    public string gardenSceneName = "Main";

    private Plant current;

    private void OnEnable()
    {
        current = PlantGameManager.Instance?.selectedPlant;

        if (current == null)
        {
            Debug.LogError("No selected plant for details scene.");
            ClearUI();
            return;
        }

        current.OnProgressChanged += HandlePlantChanged;
        UpdateUI();
    }

    private void HandlePlantChanged(Plant p)
    {
        if (p == current)
            UpdateUI();
    }

    private void OnDisable()
    {
        if (current != null)
            current.OnProgressChanged -= HandlePlantChanged;
    }

    private void HandleSelectedPlantChanged(Plant p)
    {
        current = p;
        if (current == null)
        {
            Debug.LogError("PlantSceneController: no plant selected!");
            ClearUI();
            return;
        }

        UpdateUI();
    }

    // updates when any plant changes (so if apply-to-all changes the selected one too)
    private void HandleAnyPlantChanged(Plant changed)
    {
        if (current == null) return;
        // if the changed plant is the currently displayed one, update UI
        if (changed == current)
            UpdateUI();
    }

    private void UpdateUI()
    {
        if (current == null)
        {
            ClearUI();
            return;
        }

        var stage = current.GetStage();
        plantImage.sprite = stage?.sprite;
        plantNameText.text = current.plantName;
        descriptionText.text = stage?.description ?? "No data";

        waterText.text = $"Water: {current.currentWater}/{(stage?.waterRequired ?? 0)}";
        daysText.text = $"Days: {current.currentDays}/{(stage?.daysRequired ?? 0)}";
        minigamesText.text = $"Minigames: {current.currentMinigames}/{(stage?.minigamesRequired ?? 0)}";

        bool ready = current.RequirementsMet();
        bool isFinal = current.IsFinalStage();

        // Hide both first (important)
        upgradeButton.gameObject.SetActive(false);
        harvestButton.gameObject.SetActive(false);

        if (ready)
        {
            if (isFinal)
            {
                harvestButton.gameObject.SetActive(true);
            }
            else
            {
                upgradeButton.gameObject.SetActive(true);
            }
        }
    }

    private void ClearUI()
    {
        plantImage.sprite = null;
        plantNameText.text = "";
        descriptionText.text = "";
        waterText.text = "";
        daysText.text = "";
        minigamesText.text = "";
        upgradeButton.gameObject.SetActive(false);
    }

    public void BackToGarden()
    {
        SceneManager.LoadScene(gardenSceneName);
    }

    public void UpgradePlant()
    {
        if (current == null)
        {
            Debug.LogError("Upgrade: current is null");
            return;
        }

        Debug.Log($"Upgrading plant {current.plantName} with ID {current.UniqueId}");

        if (current.IsFinalStage())
        {
            HarvestPlant();
            return;
        }

        if (current.Upgrade())
        {
            PlantSaveSystem.SaveAll(PlantGameManager.Instance.allPlants);
            UpdateUI();
        }
    }

    private void HarvestPlant()
    {
        Debug.Log($"Harvesting plant {current.plantName} ({current.UniqueId})");

        // 1. Adds the item count to inventory (stays hidden in book)
        PlayerInventory.Instance.AddItem(current.plantName, 0);

        // 2. UNLOCKS the silhouette in the Botany Book
        PlayerInventory.Instance.DiscoverPlant(current.plantName);

        string plotId = current.plotId;

        // Remove from manager tracking
        if (PlantGameManager.Instance.plantsByPlot.ContainsKey(plotId))
        {
            PlantGameManager.Instance.plantsByPlot.Remove(plotId);
        }

        PlantGameManager.Instance.allPlants.Remove(current);

        // Destroy the plant object
        Destroy(current.gameObject);

        // Save updated state so the plant stays gone when they reload
        PlantSaveSystem.SaveAll(PlantGameManager.Instance.allPlants);

        // Return to garden
        SceneManager.LoadScene(gardenSceneName);
    }
}
