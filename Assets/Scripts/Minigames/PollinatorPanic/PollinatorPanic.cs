using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollinatorPanic : MiniGame
{
    [Header("Prefabs")]
    public GameObject realFlowerPrefab;
    public GameObject fakeFlowerPrefab;
    public GameObject beePrefab;

    [Header("Spawn Area")]
    public BoxCollider2D spawnArea;

    [Header("Spawn Settings")]
    public int realFlowerCount = 4;
    public int fakeFlowerCount = 3;
    public float minSpacing = 1.5f;

    private int pollinatedCount = 0;
    private List<Vector3> usedPositions = new List<Vector3>();

    public override void StartGame(float duration)
    {
        base.StartGame(duration);

        SpawnFlowers();
        SpawnBee();

        StartCoroutine(RoundTimer());
    }

    void SpawnFlowers()
    {
        usedPositions.Clear();

        for (int i = 0; i < realFlowerCount; i++)
        {
            Instantiate(realFlowerPrefab, GetValidPosition(), Quaternion.identity, transform);
        }

        for (int i = 0; i < fakeFlowerCount; i++)
        {
            Instantiate(fakeFlowerPrefab, GetValidPosition(), Quaternion.identity, transform);
        }
    }

    void SpawnBee()
    {
        Instantiate(beePrefab, Vector3.zero, Quaternion.identity, transform);
    }

    Vector3 GetValidPosition()
    {
        Bounds bounds = spawnArea.bounds;

        Vector3 pos;
        int attempts = 0;

        do
        {
            pos = new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                0f
            );

            attempts++;

        } while (!IsFarEnough(pos) && attempts < 30);

        usedPositions.Add(pos);
        return pos;
    }

    bool IsFarEnough(Vector3 pos)
    {
        foreach (Vector3 existing in usedPositions)
        {
            if (Vector3.Distance(pos, existing) < minSpacing)
                return false;
        }

        return true;
    }

    public void Pollinate()
    {
        pollinatedCount++;

        if (pollinatedCount >= realFlowerCount)
            Win();
    }

    public void HitFake()
    {
        Fail();
    }

    IEnumerator RoundTimer()
    {
        yield return new WaitForSeconds(roundTime);

        if (IsActive)
            Fail();
    }
}