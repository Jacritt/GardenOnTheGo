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
        if (current == null)
        {
            Debug.LogError("Upgrade: current is null");
            return;
        }

        Debug.Log($"Upgrading plant {current.plantName} with ID {current.UniqueId}");

        if (current.Upgrade())
        {
            PlantSaveSystem.SaveAll(PlantGameManager.Instance.allPlants);
            UpdateUI();
        }
    }
}
