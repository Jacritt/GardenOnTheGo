// MiniGameButton.cs
using UnityEngine;

public class MinigameTracker : MonoBehaviour
{
    public Plant targetPlant;
    public bool applyToAll = false;

    public void AddMinigame()
    {
        if (applyToAll)
        {
            Debug.LogError("MiniGameButton added value");
            PlantGameManager.Instance?.AddMinigameToAll();
            return;
        }

        Plant p = targetPlant ?? PlantGameManager.Instance?.selectedPlant;
        if (p == null)
        {
            Debug.LogError("MiniGameButton: no target and no selected plant!");
            return;
        }
        PlantGameManager.Instance.AddMinigame(p);
    }
}
