using UnityEngine;
using System.Collections.Generic;

public class DormancyNap : MiniGame
{
    public enum SeasonTarget { Winter, Spring, Summer, Fall }

    [Header("References")]
    public SeasonSpinner spinner;
    public List<DormancyPlant> plants;

    [Header("Current Game State")]
    public SeasonTarget targetSeason;
    private int plantsAwakened = 0;
    private int plantsRequiredToWin = 2; // Since each season has two plants

    void Start()
    {
        // For testing - in your real game, this might be called by a Level Loader
        StartGame(15f);
    }

    public override void StartGame(float duration)
    {
        base.StartGame(duration);
        plantsAwakened = 0; // <--- Ensure this line is here!

        spinner.StartRandomSpin();
        Invoke("HandleSpinResult", spinner.spinDuration + 0.5f);
    }

    public void HandleSpinResult()
    {
        // Match our internal target to whatever the spinner picked
        targetSeason = (SeasonTarget)spinner.currentSeason;
        Debug.Log("New Goal: Find the " + targetSeason + " plants!");
    }

    public void PlantClicked(DormancyPlant clickedPlant)
    {
        // Don't allow clicking while the arrow is still moving
        if (!IsActive || spinner.isSpinning) return;

        if (clickedPlant.mySeason == targetSeason)
        {
            Debug.Log("Correct! Plant is waking up.");
            clickedPlant.WakeUp();
            plantsAwakened++;
            CheckForWin();
        }
        else
        {
            Debug.Log("WRONG SEASON! Game Over.");
            clickedPlant.Wither();
            Fail();
        }
    }

    void CheckForWin()
    {
        if (plantsAwakened >= plantsRequiredToWin)
        {
            Debug.Log("You matched all plants for this season!");
            Win();
        }
    }
}