using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject applePrefab;     // prefab for apple (assign)
    public Transform spawnAreaLeft;    // left boundary (world position)
    public Transform spawnAreaRight;   // right boundary
    public Transform dropPointParent;  // optional parent for spawned apples
    public float spawnInterval = 0.7f;
    public float appleFallSpeedMin = 1.5f;
    public float appleFallSpeedMax = 3.5f;

    public Action onAppleMissed;
    public Action onAppleCaught;

    bool isSpawning = false;
    Coroutine spawnRoutine;

    // Start spawning for a given duration - will stop automatically after duration
    public void BeginSpawning(float duration)
    {
        if (isSpawning) StopSpawning();
        isSpawning = true;
        spawnRoutine = StartCoroutine(SpawnAndStopAfter(duration));
    }

    IEnumerator SpawnAndStopAfter(float duration)
    {
        float timer = 0f;
        while (timer < duration && isSpawning)
        {
            SpawnOne();
            yield return new WaitForSeconds(spawnInterval);
            timer += spawnInterval;
        }
        StopSpawning();
    }

    void SpawnOne()
    {
        if (applePrefab == null) return;
        Vector3 left = spawnAreaLeft != null ? spawnAreaLeft.position : transform.position + Vector3.left * 2f;
        Vector3 right = spawnAreaRight != null ? spawnAreaRight.position : transform.position + Vector3.right * 2f;
        float x = UnityEngine.Random.Range(left.x, right.x);
        Vector3 spawnPos = new Vector3(x, transform.position.y, 0f);
        GameObject go = Instantiate(applePrefab, spawnPos, Quaternion.identity, dropPointParent);
        Apple a = go.GetComponent<Apple>();
        if (a != null)
        {
            float fallSpeed = UnityEngine.Random.Range(appleFallSpeedMin, appleFallSpeedMax);
            a.Initialize(fallSpeed, this);
            a.onCaught += () => onAppleCaught?.Invoke();
            a.onMissed += () => onAppleMissed?.Invoke();
        }
    }

    public void StopSpawning()
    {
        isSpawning = false;
        if (spawnRoutine != null) StopCoroutine(spawnRoutine);
        spawnRoutine = null;
    }
}
