using UnityEngine;

public class DormancyPlant : MonoBehaviour
{
    [Header("Settings")]
    public DormancyNap gameManager;
    public DormancyNap.SeasonTarget mySeason; // Set this in Inspector (Winter, Spring, etc)

    [Header("Visuals")]
    public SpriteRenderer spriteRenderer;
    public Sprite dormantSprite;
    public Sprite awakeSprite;
    public Sprite witheredSprite;

    private bool isDone = false;

    void OnMouseDown()
    {
        if (isDone) return;
        gameManager.PlantClicked(this);
    }

    public void WakeUp()
    {
        isDone = true;
        spriteRenderer.sprite = awakeSprite;
        // Optional: Add a "Bloom" sound or particle here
    }

    public void Wither()
    {
        isDone = true;
        spriteRenderer.sprite = witheredSprite;
        spriteRenderer.color = Color.gray; // Make it look dead
    }
}