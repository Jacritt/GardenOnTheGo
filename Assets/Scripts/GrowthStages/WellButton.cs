// WellButton.cs
using UnityEngine;

public class WellButton : MonoBehaviour
{
    [Tooltip("Leave null to apply water to the selected plant.")]
    public Plant targetPlant;

    [Tooltip("If true, apply to all plants instead of target/selected.")]
    public bool applyToAll = false;

    public void AddWater()
    {
        if (applyToAll)
        {
            PlantGameManager.Instance?.AddWaterToAll();
            return;
        }

        Plant p = targetPlant ?? PlantGameManager.Instance?.selectedPlant;
        if (p == null)
        {
            Debug.LogError("WellButton: no target and no selected plant!");
            return;
        }
        PlantGameManager.Instance.AddWater(p);
    }
}
