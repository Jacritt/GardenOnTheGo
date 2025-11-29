// Plant.cs
using System;
using UnityEngine;

[DisallowMultipleComponent]
public class Plant : MonoBehaviour
{
    [Header("Identity (must be unique per prefab/instance)")]
    [Tooltip("If empty: a GUID will be generated on first play and saved to PlayerPrefs for this instance.")]
    [SerializeField] private string uniqueId = "";
    public GameObject prefabSource;   // the prefab used to create this instance

    public string UniqueId
    {
        get
        {
            if (string.IsNullOrEmpty(uniqueId))
            {
                uniqueId = Guid.NewGuid().ToString();
            }
            return uniqueId;
        }
        set => uniqueId = value;
    }

    [Header("Data")]
    public string plantName = "Plant";
    public GrowthStage[] growthStages;

    [Header("Runtime progress")]
    public int currentStage = 0;
    public int currentWater = 0;
    public int currentDays = 0;
    public int currentMinigames = 0;

    [Header("Plot assignment")]
    public string plotId = "";

    public event Action<Plant> OnProgressChanged; // invoked when any progress changes (for UI hooks)

    private void Awake()
    {
        // Ensure unique id exists (persisted to the serialized field so it remains between runs in editor builds).
        if (string.IsNullOrEmpty(uniqueId))
        {
            uniqueId = Guid.NewGuid().ToString();
        }
    }

    public GrowthStage GetStage()
    {
        if (growthStages == null || growthStages.Length == 0)
            return null;

        int idx = Mathf.Clamp(currentStage, 0, growthStages.Length - 1);
        return growthStages[idx];
    }

    public bool RequirementsMet()
    {
        var s = GetStage();
        if (s == null) return false;
        return currentWater >= s.waterRequired &&
               currentDays >= s.daysRequired &&
               currentMinigames >= s.minigamesRequired;
    }

    // Upgrade returns true if upgraded
    public bool Upgrade()
    {
        if (growthStages == null) return false;
        if (currentStage + 1 >= growthStages.Length) return false;

        currentStage++;
        currentWater = 0;
        currentDays = 0;
        currentMinigames = 0;

        OnProgressChanged?.Invoke(this);
        return true;
    }

    // methods to mutate and notify
    public void AddWater(int amount = 1)
    {
        currentWater += amount;
        OnProgressChanged?.Invoke(this);
    }

    public void AddDay(int amount = 1)
    {
        currentDays += amount;
        OnProgressChanged?.Invoke(this);
    }

    public void AddMinigame(int amount = 1)
    {
        currentMinigames += amount;
        OnProgressChanged?.Invoke(this);
    }

    // Apply saved state (used during load)
    public void ApplySave(int stage, int water, int days, int minis)
    {
        currentStage = stage;
        currentWater = water;
        currentDays = days;
        currentMinigames = minis;
        OnProgressChanged?.Invoke(this);
        Debug.Log($"[Plant] Applied save to {gameObject.name}: Stage {stage}, Water {water}, Days {days}, Minis {minis}");
    }
}
