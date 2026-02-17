using System.Collections.Generic;
using UnityEngine;

public class WeedHuntMiniGame : MiniGame
{
    private List<Vector3> usedPositions = new List<Vector3>();
    public float minSpacing = 1.3f;


    [Header("Sprites")]
    public Sprite[] weedSprites;
    public Sprite[] flowerSprites;

    [Header("Spawn Settings")]
    public GameObject gardenItemPrefab;
    public int weedCount = 3;
    public int flowerCount = 5;
    public Vector2 spawnArea = new Vector2(4f, 2.5f);

    private int weedsRemaining;

    public override void StartGame(float duration)
    {
        base.StartGame(duration);

        usedPositions.Clear();
        SetupRound();
    }

    void SetupRound()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        weedsRemaining = weedCount;

        for (int i = 0; i < weedCount; i++)
            SpawnItem(true);

        for (int i = 0; i < flowerCount; i++)
            SpawnItem(false);
    }

    void SpawnItem(bool isWeed)
    {
        Vector3 pos;
        int attempts = 0;

        // Try to find a non-overlapping position
        do
        {
            pos = new Vector3(
                Random.Range(-spawnArea.x, spawnArea.x),
                Random.Range(-spawnArea.y, spawnArea.y),
                isWeed ? -1f : 0f   // Weeds closer to camera
            );

            attempts++;

        } while (IsTooClose(pos) && attempts < 30);

        usedPositions.Add(pos);

        GameObject obj = Instantiate(gardenItemPrefab, pos, Quaternion.identity, transform);

        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();

        // Assign sprite
        if (isWeed)
        {
            sr.sprite = weedSprites[Random.Range(0, weedSprites.Length)];
            sr.sortingOrder = 10;   // Render on top
        }
        else
        {
            sr.sprite = flowerSprites[Random.Range(0, flowerSprites.Length)];
            sr.sortingOrder = 0;
        }

        // Scale correctly
        float targetSize = 1.2f;
        float spriteHeight = sr.sprite.bounds.size.y;
        float scaleFactor = targetSize / spriteHeight;
        obj.transform.localScale = Vector3.one * scaleFactor;

        // Initialize
        obj.GetComponent<GardenItem>().Init(this, isWeed);
    }


    bool IsTooClose(Vector3 newPos)
    {
        foreach (Vector3 pos in usedPositions)
        {
            if (Vector3.Distance(pos, newPos) < minSpacing)
                return true;
        }
        return false;
    }


    public void WeedClicked()
    {
        if (!IsActive) return;

        weedsRemaining--;

        if (weedsRemaining <= 0)
            Win();
    }

    public void FlowerClicked()
    {
        if (!IsActive) return;

        Fail();
    }
}
