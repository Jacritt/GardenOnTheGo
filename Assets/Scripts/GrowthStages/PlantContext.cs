// PlantContext.cs
// Simple static storage to transfer selected plant info to the PlantDetailScene.
// This is lightweight and avoids PlayerPrefs for transient data.

using UnityEngine;

public static class PlantContext
{
    // We'll pack only what the detail scene needs to show and possibly edit.
    public static string plantName;
    public static Sprite stageSprite;
    public static string stageDescription;
    public static int waterRequired;
    public static int daysRequired;
    public static int minigamesRequired;

    // Current progress values (so UI can show "0/1")
    public static int currentWater;
    public static int currentDaysPast;
    public static int currentMinigamesPlayed;

    public static void Clear()
    {
        plantName = null;
        stageSprite = null;
        stageDescription = null;
        waterRequired = daysRequired = minigamesRequired = 0;
        currentWater = currentDaysPast = currentMinigamesPlayed = 0;
    }
}

