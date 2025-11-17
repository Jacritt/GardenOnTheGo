using UnityEngine;

[CreateAssetMenu(fileName = "PlantData", menuName = "Garden/Plant Data")]
public class PlantData : ScriptableObject
{
    public string plantName;
    public Sprite seedSprite;
    public Sprite saplingSprite;
    public Sprite matureSprite;
    public Sprite witheredSprite;

    public float growthTimeDays = 1f; // time to next stage
    public int minigameRequired = 1; // number of minigames to complete
}

