using UnityEngine;

public class MiniGame : MonoBehaviour
{
    public bool IsActive { get; protected set; } = false;
    protected float roundTime = 4f;

    // Called by GameManager after instantiation
    public virtual void StartGame(float duration)
    {
        IsActive = true;
        roundTime = duration;
        // mini-games can override and set up UI/animations
    }

    // helper for mini-games to call on win
    protected void Win()
    {
        if (!IsActive) return;
        IsActive = false;
        GameManager.Instance.WinGame();
    }

    // helper for mini-games to call on fail
    protected void Fail()
    {
        if (!IsActive) return;
        IsActive = false;
        GameManager.Instance.LoseLife();
    }

    // optional cleanup hook
    protected virtual void OnDestroy()
    {
        IsActive = false;
    }
}

