using UnityEngine;

public class PlantDebugTools : MonoBehaviour
{
    public Plant plant;

    [ContextMenu("Force Advance Stage")]
    public void ForceAdvance()
    {
        plant.ResetSave(); // start fresh
        // cheat: set stageStart to 2 days ago and mark water+minigame completed
        var sdType = typeof(Plant);
        // easiest path: call Water() and NotifyMinigameCompleted then override stageStartTicks using reflection or via ResetSave then manual modification
        Debug.Log("Use ResetSave or change code to force values in tests.");
    }
}

