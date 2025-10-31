using System.Collections;
using UnityEngine;

public class AppleGame : MiniGame
{
    [Header("Apple Game Settings")]
    public AppleSpawner spawner;
    public int targetCatch = 5;     // apples required to win
    public int maxMisses = 3;       // misses allowed before fail
    public Animator characterAnimator; // optional

    int caught = 0;
    int missed = 0;

    public override void StartGame(float duration)
    {
        base.StartGame(duration);
        caught = 0;
        missed = 0;

        if (spawner != null)
        {
            spawner.BeginSpawning(roundTime);
            spawner.onAppleMissed += OnAppleMissed;
            spawner.onAppleCaught += OnAppleCaught;
        }

        if (characterAnimator) characterAnimator.SetTrigger("Idle");
    }

    void OnAppleCaught()
    {
        if (!IsActive) return;
        caught++;
        if (characterAnimator) characterAnimator.SetTrigger("ThumbsUp");

        if (caught >= targetCatch)
        {
            // win
            StopSpawnerAndCleanup();
            StartCoroutine(DelayedWin(0.25f));
        }
    }

    void OnAppleMissed()
    {
        if (!IsActive) return;
        missed++;
        if (characterAnimator) characterAnimator.SetTrigger("React"); // optional reaction

        if (missed >= maxMisses)
        {
            StopSpawnerAndCleanup();
            StartCoroutine(DelayedFail(0.25f));
        }
    }

    IEnumerator DelayedWin(float t)
    {
        yield return new WaitForSeconds(t);
        Win();
    }

    IEnumerator DelayedFail(float t)
    {
        yield return new WaitForSeconds(t);
        Fail();
    }

    void StopSpawnerAndCleanup()
    {
        if (spawner != null)
        {
            spawner.onAppleMissed -= OnAppleMissed;
            spawner.onAppleCaught -= OnAppleCaught;
            spawner.StopSpawning();
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (spawner != null)
        {
            spawner.onAppleMissed -= OnAppleMissed;
            spawner.onAppleCaught -= OnAppleCaught;
        }
    }
}
