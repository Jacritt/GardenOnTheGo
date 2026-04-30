using UnityEngine;
using System.Collections.Generic;

public class TimberTurbo : MiniGame
{
    [Header("Tree Objects")]
    public List<Transform> treeTransforms; // Drag your 3 tree objects here

    [Header("Growth Settings")]
    public float growthPerClick = 0.07f;
    public float decaySpeed = 0.03f; // Optional: Trees shrink slowly if you stop clicking
    public float winScale = 1.0f;

    [Header("Growth Visuals")]
    public Sprite seedlingSprite;
    public Sprite saplingSprite;
    public Sprite fullTreeSprite;

    [Header("Thresholds (0.0 to 1.0)")]
    public float saplingThreshold = 0.4f;
    public float fullTreeThreshold = 0.8f;

    private float[] growthAmounts; // Stores the progress of each tree

    // For testing, we start the game automatically
    void Start()
    {
        StartGame(10f);
    }

    public override void StartGame(float duration)
    {
        base.StartGame(duration);

        growthAmounts = new float[treeTransforms.Count];

        for (int i = 0; i < treeTransforms.Count; i++)
        {
            growthAmounts[i] = 0.1f; // Start at 10% growth
            UpdateTreeVisual(i);     // Set initial sprite
        }
    }

    void Update()
    {
        if (!IsActive) return;

        // 1. Listen for clicks
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }

        // 2. Continuous Decay & Scaling
        for (int i = 0; i < treeTransforms.Count; i++)
        {
            // Only decay if it's not fully grown yet
            if (growthAmounts[i] > 0.1f && growthAmounts[i] < 1.0f)
            {
                growthAmounts[i] -= decaySpeed * Time.deltaTime;
            }

            // --- THE SCALE FIX ---
            // We calculate the X and Y based on your starting values
            // This keeps the original aspect ratio (3.07 x 3.88)
            float currentProgress = Mathf.Clamp(growthAmounts[i], 0.1f, 1.0f);

            // 3.07 is your start, we add growth on top of it
            float finalX = 3.0796f + (currentProgress * 2.0f);
            float finalY = 3.8884f + (currentProgress * 2.5f);

            treeTransforms[i].localScale = new Vector3(finalX, finalY, 1);

            UpdateTreeVisual(i);
        }
    }

    void HandleClick()
    {
        // Check what the mouse clicked on
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null)
        {
            for (int i = 0; i < treeTransforms.Count; i++)
            {
                if (hit.collider.transform == treeTransforms[i])
                {
                    // Add growth
                    growthAmounts[i] += growthPerClick;

                    // Cap at 1.0
                    if (growthAmounts[i] > 1.0f) growthAmounts[i] = 1.0f;

                    CheckWinCondition();
                    break;
                }
            }
        }
    }

    void UpdateTreeVisual(int index)
    {
        SpriteRenderer renderer = treeTransforms[index].GetComponent<SpriteRenderer>();
        if (renderer == null) return;

        float progress = growthAmounts[index];

        if (progress >= fullTreeThreshold)
        {
            renderer.sprite = fullTreeSprite;
        }
        else if (progress >= saplingThreshold)
        {
            renderer.sprite = saplingSprite;
        }
        else
        {
            renderer.sprite = seedlingSprite;
        }
    }

    void CheckWinCondition()
    {
        int finishedTrees = 0;
        foreach (float val in growthAmounts)
        {
            if (val >= 1.0f) finishedTrees++;
        }

        if (finishedTrees >= treeTransforms.Count)
        {
            Debug.Log("REFORESTATION COMPLETE!");
            Win();
        }
    }
}