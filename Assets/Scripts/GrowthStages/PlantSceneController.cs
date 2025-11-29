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

    [Header("Return")]
    public string gardenSceneName = "Main";

    private Plant current;

    private void OnEnable()
    {
        Debug.Log("PlantSceneController received plant: " + current);

        // 1. Try context
        var inst = PlantContext.selectedPlant;
        if (inst != null)
            current = inst.prefabSource.GetComponent<Plant>();  // Use prefab version

        // FAIL
        if (current == null)
        {
            Debug.LogError("PlantSceneController: no plant selected!");
            ClearUI();
            return;
        }

        // Init UI
        UpdateUI();
    }

    private void OnDisable()
    {
        if (PlantGameManager.Instance != null)
        {
            PlantGameManager.Instance.OnSelectedPlantChanged -= HandleSelectedPlantChanged;
            PlantGameManager.Instance.OnAnyPlantChanged -= HandleAnyPlantChanged;
        }
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

        upgradeButton.gameObject.SetActive(current.RequirementsMet());
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
        if (current == null) return;
        if (current.Upgrade())
        {
            // maybe a little animation / feedback could go here
            PlantSaveSystem.SaveAll(PlantGameManager.Instance.allPlants);
            UpdateUI();
        }
    }
}
