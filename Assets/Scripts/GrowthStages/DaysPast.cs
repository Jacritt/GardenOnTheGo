using UnityEngine;

public class DayButton : MonoBehaviour
{
    public void AddDay()
    {
        PlantContext.currentDaysPast++;
        Debug.Log("Day increased! Now: " + PlantContext.currentDaysPast);
    }
}

