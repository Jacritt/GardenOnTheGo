using UnityEngine;
using UnityEngine.UI;
using System;

public class PlantUI : MonoBehaviour
{
    public Plant plant;
    public Text stageText;
    public Text timerText;
    public Text waterText;
    public Button waterButton;
    public Button playMinigameButton;
    public Button tryAdvanceButton;

    void Start()
    {
        if (plant == null) Debug.LogWarning("Plant not assigned in PlantUI");
        if (waterButton) waterButton.onClick.AddListener(OnWaterClicked);
        if (playMinigameButton) playMinigameButton.onClick.AddListener(OnPlayMinigameClicked);
        if (tryAdvanceButton) tryAdvanceButton.onClick.AddListener(OnTryAdvanceClicked);

        UpdateUI();
    }

    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        if (plant == null) return;

        stageText.text = plant.GetStage().ToString();
        waterText.text = $"Water: {plant.GetWaterCountSinceStageStart()} / {GetRequiredWater()}";
        DateTime start = plant.GetStageStartTime();
        TimeSpan passed = DateTime.UtcNow - start;
        timerText.text = $"Days since stage start: {passed.TotalDays:F2}";

        // enable or disable buttons
        waterButton.interactable = !plant.IsWithered();
        playMinigameButton.interactable = !plant.IsWithered();
    }

    int GetRequiredWater()
    {
        switch (plant.GetStage())
        {
            case Plant.PlantStage.Seed: return plant.waterRequiredSeed;
            case Plant.PlantStage.Sapling: return plant.waterRequiredSapling;
            case Plant.PlantStage.Adult: return plant.waterRequiredAdult;
            default: return 0;
        }
    }

    void OnWaterClicked()
    {
        plant.Water();
    }
    public GameObject minigamePanel; // Assign in Inspector

    void OnPlayMinigameClicked()
    {
        minigamePanel.SetActive(true);
        var manager = minigamePanel.GetComponent<PlantMinigameManager>();
        manager.SetPlant(plant);

        plant.NotifyMinigameCompleted();
    }

    void OnTryAdvanceClicked()
    {
        bool advanced = plant.TryAdvanceStage();
        if (!advanced)
        {
            Debug.Log("Cannot advance yet: requirements not met.");
        }
    }
}

