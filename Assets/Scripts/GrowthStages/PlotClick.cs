using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlotClick : MonoBehaviour, IPointerClickHandler
{
    [Tooltip("Assign the plant placed on this plot (the Plant component on the prefab instance).")]
    public Plant plantOnPlot;

    [Tooltip("Name of the plant instance (optional)")]
    public string plantDisplayName = "Plant";

    [Tooltip("Name of the scene to load when inspecting a plant")]
    public string plantDetailSceneName = "PlantDetailScene";

    // when using Unity UI, IPointerClickHandler works if this GameObject has proper graphics/raycast target,
    // otherwise you can call OnMouseDown() if using 2D collider clicks.
    public void OnPointerClick(PointerEventData eventData)
    {
        if (plantOnPlot == null)
        {
            Debug.LogWarning("No plant assigned to plot.");
            return;
        }

        // Build the PlantContext payload
        var stage = plantOnPlot.GetCurrentStageData();
        if (stage == null)
        {
            Debug.LogWarning("Plant has no growth stages defined.");
            return;
        }

        PlantContext.plantName = plantDisplayName;
        PlantContext.stageSprite = stage.sprite;
        PlantContext.stageDescription = stage.description;
        PlantContext.waterRequired = stage.waterRequired;
        PlantContext.daysRequired = stage.daysRequired;
        PlantContext.minigamesRequired = stage.minigamesRequired;

        PlantContext.currentWater = plantOnPlot.currentWater;
        PlantContext.currentDaysPast = plantOnPlot.currentDaysPast;
        PlantContext.currentMinigamesPlayed = plantOnPlot.currentMinigamesPlayed;

        // Load detail scene
        SceneManager.LoadScene(plantDetailSceneName);
    }

    // Alternative: if you want collider clicks instead of UI pointer:
    private void OnMouseDown()
    {
        // optional: only respond if not pointer over UI
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;
        // call the pointer click handler
        OnPointerClick(null);
    }
}

