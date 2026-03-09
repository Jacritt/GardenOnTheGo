// DayButton.cs
using UnityEngine;

public class DaysPast : MonoBehaviour
{
    public bool applyToAll = false;

    public void AddDay()
    {
        if (PlantGameManager.Instance == null)
        {
            Debug.LogError("DaysPast: PlantGameManager missing!");
            return;
        }

        if (applyToAll)
        {
            PlantGameManager.Instance.AddDayToAll();
            return;
        }

        Plant p = PlantGameManager.Instance.selectedPlant;

        if (p == null)
        {
            Debug.LogError("DaysPast: no selected plant!");
            return;
        }

        PlantGameManager.Instance.AddDay(p);
        Debug.Log($"Added day to plant instance {p.UniqueId} ({p.plantName}) → Days: {p.currentDays}");
    }
}
