using UnityEngine;

public class PlantData : MonoBehaviour
{
    public static PlantData Instance;

    public int currentStage = 0;

    public int water;
    public int waterRequired = 1;

    public int daysPassed;
    public int daysRequired = 5;

    public int minigamesPlayed;
    public int minigamesRequired = 10;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // stays between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool RequirementsMet()
    {
        return water >= waterRequired &&
               daysPassed >= daysRequired &&
               minigamesPlayed >= minigamesRequired;
    }
}

