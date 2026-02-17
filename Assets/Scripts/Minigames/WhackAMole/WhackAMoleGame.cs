using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WhackAMoleGame : MiniGame
{
    [Header("Game Elements")]
    public List<GameObject> moles;
    public List<GameObject> roses;

    private int rosesRemaining;

    public override void StartGame(float duration)
    {
        base.StartGame(duration);
        rosesRemaining = roses.Count;

        // Reset visuals
        foreach (GameObject mole in moles) mole.SetActive(false);
        foreach (GameObject rose in roses) rose.SetActive(true);

        InvokeRepeating(nameof(SpawnRandomMole), 0.5f, 0.8f);

        // START THE "BEAT THE CLOCK" TIMER
        // We subtract 0.1f so we call Win() just before GameManager calls LoseLife()
        StartCoroutine(SurvivalCountdown(duration - 0.1f));
    }

    IEnumerator SurvivalCountdown(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        // If we are still alive, we win!
        if (IsActive && rosesRemaining > 0)
        {
            FinishMinigame(true);
        }
    }

    void FinishMinigame(bool success)
    {
        CancelInvoke();
        StopAllCoroutines();

        if (success) Win(); // Adds score +1
        else Fail();        // Subtracts life
    }

    // --- MOLE LOGIC ---

    void SpawnRandomMole()
    {
        if (!IsActive) return;
        int index = Random.Range(0, moles.Count);

        if (roses[index].activeSelf && !moles[index].activeSelf)
        {
            moles[index].SetActive(true);
            StartCoroutine(MoleTimer(index));
        }
    }

    IEnumerator MoleTimer(int index)
    {
        yield return new WaitForSeconds(1.1f);

        if (moles[index].activeSelf && IsActive)
        {
            moles[index].SetActive(false);
            roses[index].SetActive(false);
            rosesRemaining--;

            if (rosesRemaining <= 0)
            {
                FinishMinigame(false);
            }
        }
    }

    public void WhackMole(int index)
    {
        if (!IsActive) return;
        moles[index].SetActive(false);
    }
}