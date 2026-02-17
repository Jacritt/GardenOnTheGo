using System.Collections.Generic;
using UnityEngine;

public class WeedHuntMiniGame : MiniGame
{
    [Header("Background")]
    public Sprite backgroundSprite;
    public Vector2 backgroundSize = new Vector2(10f, 6f);

    [Header("Sprites")]
    public Sprite[] weedSprites;
    public Sprite[] flowerSprites;

    [Header("Spawn Settings")]
    public GameObject gardenItemPrefab;
    public int weedCount = 3;
    public int flowerCount = 5;
    public Vector2 spawnArea = new Vector2(4f, 2.5f);

    private int weedsRemaining;
    private GameObject backgroundObject;

    public override void StartGame(float duration)
    {
        base.StartGame(duration);
        SetupRound();
    }

    void SetupRound()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        SetupBackground();

        weedsRemaining = weedCount;

        for (int i = 0; i < weedCount; i++)
            SpawnItem(true);

        for (int i = 0; i < flowerCount; i++)
            SpawnItem(false);
    }

    void SetupBackground()
    {
        if (backgroundSprite == null) return;

        backgroundObject = new GameObject("Background");
        backgroundObject.transform.SetParent(transform);
        backgroundObject.transform.localPosition = new Vector3(0f, 0f, 5f);

        SpriteRenderer sr = backgroundObject.AddComponent<SpriteRenderer>();
        sr.sprite = backgroundSprite;
        sr.sortingOrder = -100;

        Vector2 spriteSize = sr.sprite.bounds.size;
        backgroundObject.transform.localScale = new Vector3(
            backgroundSize.x / spriteSize.x,
            backgroundSize.y / spriteSize.y,
            1f
        );
    }

    void SpawnItem(bool isWeed)
    {
        Vector3 pos;
        int attempts = 0;

        do
        {
            pos = new Vector3(
                Random.Range(-spawnArea.x, spawnArea.x),
                Random.Range(-spawnArea.y, spawnArea.y),
                isWeed ? -1f : 0f
            );

            attempts++;

        } while (IsOverlapping(pos) && attempts < 40);

        GameObject obj = Instantiate(gardenItemPrefab, pos, Quaternion.identity, transform);

        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();

        if (isWeed)
        {
            sr.sprite = weedSprites[Random.Range(0, weedSprites.Length)];
            sr.sortingOrder = 10;
        }
        else
        {
            sr.sprite = flowerSprites[Random.Range(0, flowerSprites.Length)];
            sr.sortingOrder = 0;
        }

        float targetSize = 1.2f;
        float spriteHeight = sr.sprite.bounds.size.y;
        float scaleFactor = targetSize / spriteHeight;
        obj.transform.localScale = Vector3.one * scaleFactor;

        // Force collider to match sprite bounds
        Collider2D col = obj.GetComponent<Collider2D>();
        if (col is BoxCollider2D box)
        {
            box.size = sr.sprite.bounds.size;
            box.offset = sr.sprite.bounds.center;
        }


        obj.GetComponent<GardenItem>().Init(this, isWeed);
    }

    bool IsOverlapping(Vector3 newPos)
    {
        Collider2D prefabCollider = gardenItemPrefab.GetComponent<Collider2D>();
        if (prefabCollider == null)
            return false;

        if (prefabCollider is CircleCollider2D circle)
        {
            float scaledRadius = circle.radius * gardenItemPrefab.transform.localScale.x;
            Collider2D hit = Physics2D.OverlapCircle(newPos, scaledRadius);
            return hit != null;
        }

        if (prefabCollider is BoxCollider2D box)
        {
            Vector2 scaledSize = Vector2.Scale(box.size, gardenItemPrefab.transform.localScale);
            Collider2D hit = Physics2D.OverlapBox(newPos, scaledSize, 0f);
            return hit != null;
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
