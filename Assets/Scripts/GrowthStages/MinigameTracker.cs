// MiniGameButton.cs
using UnityEngine;

public class MinigameTracker : MonoBehaviour
{
    public Plant targetPlant;
    public bool applyToAll = false;

    public void AddMinigame()
    {
        // Check if we apply to all or single plant
        if (applyToAll)
        {
            PlantGameManager.Instance?.AddMinigameToAll();
            return;
        }

        Plant p = targetPlant ?? PlantGameManager.Instance?.selectedPlant;
        if (p == null)
        {
            Debug.LogError("MiniGameButton: no target and no selected plant!");
            return; // <- prevents null reference
        }

        PlantGameManager.Instance.AddMinigame(p);
        Debug.Log($"Added minigame to {p.plantName}. Total: {p.currentMinigames}");
    }
}

