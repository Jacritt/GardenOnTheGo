using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GrowthStage
{
    public string stageName;
    public Sprite sprite;
    [Tooltip("Description shown in plant detail scene")]
    [TextArea]
    public string description;

    [Header("Requirements (editable per stage)")]
    public int waterRequired = 1;
    public int daysRequired = 5;
    public int minigamesRequired = 10;
}

public class Plant : MonoBehaviour
{
    [Header("Growth stages (index = stage number)")]
    public List<GrowthStage> growthStages = new List<GrowthStage>();

    [Header("Current stage index (0-based)")]
    public int currentStage = 0;

    // Optional runtime tracking (not persisted across scenes unless you implement saving)
    [Header("Runtime progress (editable if you want to test)")]
    public int currentWater = 0;
    public int currentDaysPast = 0;
    public int currentMinigamesPlayed = 0;

    private void Reset()
    {
        // helpful default so inspector isn't empty
        growthStages = new List<GrowthStage> { new GrowthStage() };
    }

    public GrowthStage GetCurrentStageData()
    {
        if (growthStages == null || growthStages.Count == 0) return null;
        int idx = Mathf.Clamp(currentStage, 0, growthStages.Count - 1);
        return growthStages[idx];
    }
}

