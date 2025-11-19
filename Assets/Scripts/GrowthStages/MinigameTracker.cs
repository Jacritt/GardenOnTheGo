using UnityEngine;

public class MiniGameButton : MonoBehaviour
{
    public void AddMinigame()
    {
        PlantContext.currentMinigamesPlayed++;
        Debug.Log("Minigame increased! Now: " + PlantContext.currentMinigamesPlayed);
    }
}

