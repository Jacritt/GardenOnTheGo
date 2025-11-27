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
    public Dictionary<string, Plant> plantById = new Dictionary<string, Plant>();
    public Dictionary<string, Plant> plantsByPlot = new Dictionary<string, Plant>();


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
        }
    }

    private void Start()
    {
        // find all plants in scene if not pre-assigned
        if (allPlants == null || allPlants.Length == 0)
        {
            allPlants = FindObjectsOfType<Plant>();
        }

        BuildLookup();

        // load saved data
        PlantSaveSystem.LoadInto(plantById);

        // Rebuild mapping from plotId ? Plant
        RebuildPlotMapping();

        // Subscribe to OnProgressChanged events for all plants
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

    private void RebuildPlotMapping()
    {
        plantsByPlot.Clear();
        foreach (var p in FindObjectsOfType<Plant>())
        {
            if (!string.IsNullOrEmpty(p.plotId))
            {
                plantsByPlot[p.plotId] = p;
                Debug.Log($"[PlantGameManager] Restored plant {p.name} for plotId {p.plotId}");
            }
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
        PlantContext.selectedPlant = p;
        OnSelectedPlantChanged?.Invoke(p);
    }

    public void RegisterPlant(Plant p)
    {
        // Expand allPlants array
        var list = new List<Plant>(allPlants);
        list.Add(p);
        allPlants = list.ToArray();

        // Add to dictionary
        if (!string.IsNullOrEmpty(p.UniqueId) && !plantById.ContainsKey(p.UniqueId))
            plantById[p.UniqueId] = p;

        // Add to plot mapping
        if (!string.IsNullOrEmpty(p.plotId))
            plantsByPlot[p.plotId] = p;

        // Select the plant
        SetSelectedPlant(p);
        PlantContext.selectedPlant = p;

        // Subscribe to autosave updates
        p.OnProgressChanged += OnPlantProgressChanged;

        // Save immediately
        PlantSaveSystem.SaveAll(allPlants);
    }

    public Plant GetPlantById(string id)
    {
        if (plantById.TryGetValue(id, out Plant p))
            return p;
        return null;
    }

    #region Mutation APIs

    public void AddWater(Plant target, int amount = 1)
    {
        if (target == null) return;
        target.AddWater(amount);
        OnPlantProgressChanged(target);
    }

    public void AddWaterToAll(int amount = 1)
    {
        foreach (var p in allPlants)
            if (p != null) AddWater(p ,amount);
    }

    public void AddMinigame(Plant target, int amount = 1)
    {
        if (target == null) return;
        target.AddMinigame(amount);
        OnPlantProgressChanged(target);
    }

    public void AddMinigameToAll(int amount = 1)
    {
        foreach (var p in allPlants)
            if (p != null) AddMinigame(p, amount);
    }

    public void AddDay(Plant target, int amount = 1)
    {
        if (target == null) return;
        target.AddDay(amount);
        OnPlantProgressChanged(target);
    }

    public void AddDayToAll(int amount = 1)
    {
        foreach (var p in allPlants)
            if (p != null) AddDay(p,amount);
    }

    #endregion

    private void OnApplicationQuit()
    {
        // final save
        PlantSaveSystem.SaveAll(allPlants);
    }
}
