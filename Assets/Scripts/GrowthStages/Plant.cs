using System;
using System.IO;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public enum PlantStage { Seed = 0, Sapling = 1, Adult = 2, Withered = 3 }

    [Header("Stage visuals (assign children)")]
    public GameObject stageSeed;
    public GameObject stageSapling;
    public GameObject stageAdult;
    public GameObject stageWithered;

    [Header("Growth settings")]
    public int daysToAdvance = 1;                // days required to wait per stage (we use 1)
    public int waterRequiredSeed = 1;            // waterings required for seed -> sapling
    public int waterRequiredSapling = 2;         // for sapling -> adult
    public int waterRequiredAdult = 3;           // adult's next (if you had more stages)
    public int daysBeforeWitherIfNotWatered = 2; // if not watered within X days, wither

    [Header("Save")]
    public string plantId = ""; // unique id for this instance

    // runtime state
    PlantSaveData saveData;
    string savePath => Path.Combine(Application.persistentDataPath, $"plant_{plantId}.json");

    // event hooks
    public Action<PlantStage> OnStageChanged;
    public Action OnWithered;

    void Awake()
    {
        if (string.IsNullOrEmpty(plantId))
        {
            // generate a unique id (useful if multiple plant prefabs)
            plantId = Guid.NewGuid().ToString();
        }
    }

    void Start()
    {
        LoadOrCreate();
        RefreshVisual();
    }

    void Update()
    {
        if (saveData.isWithered) return;

        // check for wither: if time since lastWatered exceeds threshold
        if (saveData.lastWateredTicks != 0)
        {
            var lastWater = new DateTime(saveData.lastWateredTicks, DateTimeKind.Utc);
            if ((DateTime.UtcNow - lastWater).TotalDays >= daysBeforeWitherIfNotWatered
                && saveData.waterCountSinceStageStart == 0)
            {
                // hasn't been watered in threshold -> wither
                Wither();
                return;
            }
        }
        else
        {
            // never watered
            var stageStart = new DateTime(saveData.stageStartTicks, DateTimeKind.Utc);
            if ((DateTime.UtcNow - stageStart).TotalDays >= daysBeforeWitherIfNotWatered)
            {
                Wither();
                return;
            }
        }

        // Optionally you can run automatic checks or UI timers here
    }

    // call when player waters the plant
    public void Water()
    {
        if (saveData.isWithered) return;

        int requiredWater = GetRequiredWaterForStage(saveData.stage);
        if (saveData.waterCountSinceStageStart >= requiredWater)
        {
            Debug.Log($"? {plantId} already has enough water for this stage!");
            return; // don’t go over the limit
        }

        saveData.lastWateredTicks = DateTime.UtcNow.Ticks;
        saveData.waterCountSinceStageStart++;
        Save();

        Debug.Log($"?? Watered plant {plantId}: {saveData.waterCountSinceStageStart}/{requiredWater}");
    }


    // call when player finishes the minigame for this stage
    public void NotifyMinigameCompleted()
    {
        if (saveData.isWithered) return;
        saveData.minigameCompletedSinceStageStart = true;
        Save();
        Debug.Log($"Minigame complete flag set for {GetPlantId()}");
    }

    // player can attempt to advance (you may call this each time they open plant UI or explicitly press a button)
    public bool TryAdvanceStage()
    {
        if (saveData.isWithered) return false;

        var stageStart = new DateTime(saveData.stageStartTicks, DateTimeKind.Utc);
        double daysPassed = (DateTime.UtcNow - stageStart).TotalDays;

        int requiredWater = GetRequiredWaterForStage(saveData.stage);
        bool waterOk = saveData.waterCountSinceStageStart >= requiredWater;
        bool minigameOk = saveData.minigameCompletedSinceStageStart;
        bool timeOk = daysPassed >= daysToAdvance;

        if (timeOk && waterOk && minigameOk)
        {
            // advance
            if (saveData.stage == PlantStage.Seed) saveData.stage = PlantStage.Sapling;
            else if (saveData.stage == PlantStage.Sapling) saveData.stage = PlantStage.Adult;
            else if (saveData.stage == PlantStage.Adult) { /* already final stage */ }

            saveData.stageStartTicks = DateTime.UtcNow.Ticks;
            saveData.waterCountSinceStageStart = 0;
            saveData.minigameCompletedSinceStageStart = false;
            Save();
            RefreshVisual();
            OnStageChanged?.Invoke(saveData.stage);
            return true;
        }

        return false;
    }

    int GetRequiredWaterForStage(PlantStage s)
    {
        switch (s)
        {
            case PlantStage.Seed: return waterRequiredSeed;
            case PlantStage.Sapling: return waterRequiredSapling;
            case PlantStage.Adult: return waterRequiredAdult;
            default: return int.MaxValue;
        }
    }

    void Wither()
    {
        saveData.isWithered = true;
        saveData.stage = PlantStage.Withered;
        Save();
        RefreshVisual();
        OnWithered?.Invoke();
    }

    void RefreshVisual()
    {
        bool isSeed = saveData.stage == PlantStage.Seed;
        bool isSapling = saveData.stage == PlantStage.Sapling;
        bool isAdult = saveData.stage == PlantStage.Adult;
        bool isWithered = saveData.isWithered || saveData.stage == PlantStage.Withered;

        if (stageSeed) stageSeed.SetActive(isSeed);
        if (stageSapling) stageSapling.SetActive(isSapling);
        if (stageAdult) stageAdult.SetActive(isAdult);
        if (stageWithered) stageWithered.SetActive(isWithered);
    }

    // persistence
    void LoadOrCreate()
    {
        if (File.Exists(savePath))
        {
            try
            {
                string json = File.ReadAllText(savePath);
                saveData = JsonUtility.FromJson<PlantSaveData>(json);
            }
            catch
            {
                // broken file -> recreate
                saveData = new PlantSaveData(plantId);
                Save();
            }
        }
        else
        {
            saveData = new PlantSaveData(plantId);
            Save();
        }
    }

    void Save()
    {
        try
        {
            string json = JsonUtility.ToJson(saveData);
            File.WriteAllText(savePath, json);
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed saving plant: " + ex);
        }
    }

    // debug helpers
    [ContextMenu("Reset Plant Save")]
    public void ResetSave()
    {
        saveData = new PlantSaveData(plantId);
        Save();
        RefreshVisual();
    }

    // Expose getters for UI
    public PlantStage GetStage() => saveData.stage;
    public DateTime GetStageStartTime() => new DateTime(saveData.stageStartTicks, DateTimeKind.Utc);
    public int GetWaterCountSinceStageStart() => saveData.waterCountSinceStageStart;
    public bool GetMinigameCompletedSinceStageStart() => saveData.minigameCompletedSinceStageStart;
    public bool IsWithered() => saveData.isWithered;
    public string GetPlantId() => plantId;
}

