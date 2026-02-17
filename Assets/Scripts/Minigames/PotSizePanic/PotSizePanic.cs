using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PotSizePanic : MiniGame
{
    [Header("Game Elements")]
    public List<PotTarget> pots;
    public List<Transform> spawnPoints; // Create 5 empty GameObjects for positions

    public override void StartGame(float duration)
    {
        base.StartGame(duration);
        ShufflePots();
        StartCoroutine(SurvivalCountdown(duration - 0.1f));
    }

    void ShufflePots()
    {
        // Create a list of available positions
        List<Vector3> positions = new List<Vector3>();
        foreach (var point in spawnPoints)
        {
            positions.Add(point.position);
        }

        // Randomly assign those positions to our pots
        foreach (var pot in pots)
        {
            int randomIndex = Random.Range(0, positions.Count);
            pot.transform.position = positions[randomIndex];
            positions.RemoveAt(randomIndex); // Don't use the same spot twice!

            // Reset the pot state for a new game
            pot.isOccupied = false;
        }
    }

    void Update()
    {
        if (!IsActive) return;

        bool allPotted = true;
        foreach (var pot in pots)
        {
            if (!pot.isOccupied)
            {
                allPotted = false;
                break;
            }
        }

        if (allPotted) Win();
    }

    IEnumerator SurvivalCountdown(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }

    
}