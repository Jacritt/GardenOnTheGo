using System;
[Serializable]
public class PlantSaveData
{
    public string id; // unique identifier for plant instance
    public Plant.PlantStage stage;
    public long stageStartTicks; // DateTime.Ticks for when current stage started
    public long lastWateredTicks;
    public int waterCountSinceStageStart;
    public bool minigameCompletedSinceStageStart;
    public bool isWithered;

    // constructor
    public PlantSaveData(string id)
    {
        this.id = id;
        stage = Plant.PlantStage.Seed;
        stageStartTicks = DateTime.UtcNow.Ticks;
        lastWateredTicks = 0;
        waterCountSinceStageStart = 0;
        minigameCompletedSinceStageStart = false;
        isWithered = false;
    }
}

