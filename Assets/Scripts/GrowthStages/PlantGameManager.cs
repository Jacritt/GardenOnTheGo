// PlantGameManager.cs
using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class PlantGameManager : MonoBehaviour
{
    public static PlantGameManager Instance { get; private set; }

    [Tooltip("All plant instances currently present (will be populated automatically at Start if left empty).")]
    public Plant[] allPlants; // optional pre-assign, otherwise auto-find at Start

    // fast lookup by UniqueId
    private Dictionary<string, Plant> plantById = new Dictionary<string, Plant>();

    public Plant selectedPlant { get; set; }

    // Events
    public event Action<Plant> OnSelectedPlantChanged;
    public event Action<Plant> OnAnyPlantChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // if not assigned, find all Plants in the scene
        if (allPlants == null || allPlants.Length == 0)
        {
            allPlants = FindObjectsOfType<Plant>();
        }

        BuildLookup();
        // load saved data
        PlantSaveSystem.LoadInto(plantById);

        // subscribe to plants' change events to re-broadcast and auto-save
        foreach (var p in allPlants)
        {
            if (p == null) continue;
            p.OnProgressChanged += OnPlantProgressChanged;
        }
    }

    private void BuildLookup()
    {
        plantById.Clear();
        foreach (var p in allPlants)
        {
            if (p == null) continue;
            if (string.IsNullOrEmpty(p.UniqueId))
            {
                // ensure UniqueId exists
                Debug.LogWarning($"Plant at {p.gameObject.name} had no id; generating one.");
                p.UniqueId = Guid.NewGuid().ToString();
            }
            if (!plantById.ContainsKey(p.UniqueId))
                plantById[p.UniqueId] = p;
            else
                Debug.LogWarning($"Duplicate plant id {p.UniqueId} on {p.name}; consider setting unique ids in inspector.");
        }
    }

    private void OnPlantProgressChanged(Plant p)
    {
        // broadcast to any listeners
        OnAnyPlantChanged?.Invoke(p);

        // autosave
        PlantSaveSystem.SaveAll(allPlants);
    }

    public void SetSelectedPlant(Plant p)
    {
        selectedPlant = p;
        OnSelectedPlantChanged?.Invoke(p);
    }

    #region Convenience mutation APIs

    public void AddWater(Plant target, int amount = 1)
    {
        if (target == null) return;
        target.AddWater(amount);
    }

    public void AddWaterToAll(int amount = 1)
    {
        foreach (var p in allPlants)
            p?.AddWater(amount);
    }

    public void AddMinigame(Plant target, int amount = 1)
    {
        if (target == null) return;
        target.AddMinigame(amount);
    }

    public void AddMinigameToAll(int amount = 1)
    {
        foreach (var p in allPlants)
            p?.AddMinigame(amount);
    }

    public void AddDay(Plant target, int amount = 1)
    {
        if (target == null) return;
        target.AddDay(amount);
    }

    public void AddDayToAll(int amount = 1)
    {
        foreach (var p in allPlants)
            p?.AddDay(amount);
    }

    #endregion

    private void OnApplicationQuit()
    {
        // final save
        PlantSaveSystem.SaveAll(allPlants);
    }
}
