using UnityEngine;

public class WellButton : MonoBehaviour
{
    public void AddWater()
    {
        PlantContext.currentWater++;
        Debug.Log("Water increased! Now: " + PlantContext.currentWater);
    }
}
