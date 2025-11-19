// DayButton.cs
using UnityEngine;

public class DaysPast : MonoBehaviour
{
    public Plant targetPlant;
    public bool applyToAll = false;

    public void AddDay()
    {
        if (applyToAll)
        {
            PlantGameManager.Instance?.AddDayToAll();
            return;
        }

        Plant p = targetPlant ?? PlantGameManager.Instance?.selectedPlant;
        if (p == null)
        {
            Debug.LogError("DayButton: no target and no selected plant!");
            return;
        }
        PlantGameManager.Instance.AddDay(p);
        Debug.Log($"Added day to {p.plantName}. Total: {p.currentDays}");
    }
}
