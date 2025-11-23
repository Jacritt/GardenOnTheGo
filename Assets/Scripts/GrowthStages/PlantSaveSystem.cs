// PlantSaveSystem.cs
using System;
using System.Collections.Generic;
using UnityEngine;

public static class PlantSaveSystem
{
    private const string SAVE_KEY = "PLANT_SAVE_DATA_v1"; // bump version if save format changes

    [Serializable]
    public class PlantSaveData
    {
        public string id;
        public int stage;
        public int water;
        public int days;
        public int minis;
    }

    [Serializable]
    private class Wrapper
    {
        public List<PlantSaveData> items = new List<PlantSaveData>();
    }

    public static void SaveAll(IEnumerable<Plant> plants)
    {
        if (plants == null)
        {
            Debug.LogWarning("[PlantSaveSystem] SaveAll called with null plants.");
            return;
        }

        var wrapper = new Wrapper();
        foreach (var p in plants)
        {
            if (p == null) continue; // skip null entries

            wrapper.items.Add(new PlantSaveData
            {
                id = p.UniqueId,
                stage = p.currentStage,
                water = p.currentWater,
                days = p.currentDays,
                minis = p.currentMinigames
            });
        }

        string json = JsonUtility.ToJson(wrapper);
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();
#if UNITY_EDITOR
    Debug.Log($"[PlantSaveSystem] Saved {wrapper.items.Count} plants.");
#endif
    }


    public static void LoadInto(Dictionary<string, Plant> plantById)
    {
        if (!PlayerPrefs.HasKey(SAVE_KEY))
        {
#if UNITY_EDITOR
            Debug.Log("[PlantSaveSystem] No save found.");
#endif
            return;
        }

        string json = PlayerPrefs.GetString(SAVE_KEY);
        if (string.IsNullOrEmpty(json)) return;

        try
        {
            var wrapper = JsonUtility.FromJson<Wrapper>(json);
            if (wrapper?.items == null) return;

            foreach (var save in wrapper.items)
            {
                if (plantById.TryGetValue(save.id, out var plant))
                {
                    plant.ApplySave(save.stage, save.water, save.days, save.minis);
                }
                else
                {
                    // Plant from save not present in scene; ignore (or log)
#if UNITY_EDITOR
                    Debug.LogWarning($"[PlantSaveSystem] Saved plant id {save.id} not found in current scene.");
#endif
                }
            }
#if UNITY_EDITOR
            Debug.Log($"[PlantSaveSystem] Loaded {wrapper.items.Count} records.");
#endif
        }
        catch (Exception e)
        {
            Debug.LogError($"[PlantSaveSystem] Failed to load: {e.Message}");
        }
    }

    public static void ClearSaves()
    {
        PlayerPrefs.DeleteKey(SAVE_KEY);
    }
}
