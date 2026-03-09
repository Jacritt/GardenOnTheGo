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
        allPlants = FindObjectsOfType<Plant>(true);

        BuildLookup();
        PlantSaveSystem.LoadInto(plantById);
        RebuildPlotMapping();

        foreach (var p in allPlants)
        {
            if (p == null) continue;

            p.OnProgressChanged -= OnPlantProgressChanged;
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
            {
                Debug.LogWarning($"Duplicate plant id detected! Generating new one.");
                p.UniqueId = Guid.NewGuid().ToString();
                plantById[p.UniqueId] = p;
            }
        }
    }

    private void RebuildPlotMapping()
    {
        plantsByPlot.Clear();

        foreach (var p in FindObjectsOfType<Plant>(true))
        {
            if (!string.IsNullOrEmpty(p.plotId))
            {
                plantsByPlot[p.plotId] = p;
                Debug.Log($"[PlantGameManager] Restored plant {p.name} for plotId {p.plotId}");
            }
        }

        foreach (var plot in FindObjectsOfType<PlotClick>(true))
        {
            plot.RefreshFromManager();
            Debug.Log($"[PlotClick] Mapping plot {plot.plotId} to plant {plot.name} (ID {plot.plotId})");
        }

    }

    private void OnPlantProgressChanged(Plant p)
    {
        // broadcast to any listeners
        OnAnyPlantChanged?.Invoke(p);

        if (p == selectedPlant)
            OnSelectedPlantChanged?.Invoke(p);

        // autosave
        PlantSaveSystem.SaveAll(FindObjectsOfType<Plant>());
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
        p.OnProgressChanged -= OnPlantProgressChanged;
        p.OnProgressChanged += OnPlantProgressChanged;

        // Save immediately
        PlantSaveSystem.SaveAll(FindObjectsOfType<Plant>());
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
        
    }

    public void AddWaterToAll(int amount = 1)
    {
        foreach (var p in allPlants)
            if (p != null) 
                p.AddWater(amount);
    }

    public void AddMinigame(Plant target, int amount = 1)
    {
        if (target == null) return;
        target.AddMinigame(amount);
       
    }

    public void AddMinigameToAll(int amount = 1)
    {
        foreach (var p in allPlants)
            if (p != null) 
                p.AddMinigame(amount);
    }

    public void AddDay(Plant target, int amount = 1)
    {
        if (target == null) return;
        target.AddDay(amount);
        
    }

    public void AddDayToAll(int amount = 1)
    {
        foreach (var p in allPlants)
            if (p != null) 
                p.AddDay(amount);
    }

    #endregion

    private void OnApplicationQuit()
    {
        // final save
        PlantSaveSystem.SaveAll(FindObjectsOfType<Plant>());
    }
}
