using UnityEngine;
using System.Collections.Generic;

public class WaterOrWreckIt : MiniGame
{
    [System.Serializable]
    public class PotType
    {
        public string name;
        public GameObject potVisual; // The actual pot sprite
        public float fillSpeed;      // Fast for skinny, slow for wide
        public Vector2 targetRange;  // The min/max fill for the green zone
    }

    [Header("Pot Configurations")]
    public List<PotType> potOptions;
    public Transform potSpawnPoint;

    [Header("Water Elements")]
    public GameObject waterFill;      // The blue sprite (Pivot set to Bottom!)
    public GameObject targetZone;     // The green bar sprite
    public ParticleSystem splashEffect;

    private PotType currentPot;
    private float currentFillAmount = 0f;
    private bool isPouring = false;
    private bool gameActive = false;

    public override void StartGame(float duration)
    {
        base.StartGame(duration);
        SetupRandomPot();
        gameActive = true;
    }

    void SetupRandomPot()
    {
        // Pick one of the 3 pots
        currentPot = potOptions[Random.Range(0, potOptions.Count)];

        // Disable all pots, then enable the chosen one
        foreach (var p in potOptions) p.potVisual.SetActive(false);
        currentPot.potVisual.SetActive(true);

        // Reset water
        currentFillAmount = 0;
        waterFill.transform.localScale = new Vector3(1, 0, 1);

        // Position the Green Zone based on the pot's specific target
        targetZone.transform.localPosition = new Vector3(0, currentPot.targetRange.x, 0);
    }

    void Update()
    {
        if (!IsActive || !gameActive) return;

        if (Input.GetMouseButtonDown(0)) isPouring = true;
        if (Input.GetMouseButtonUp(0)) StopPouring();

        if (isPouring)
        {
            currentFillAmount += currentPot.fillSpeed * Time.deltaTime;
            waterFill.transform.localScale = new Vector3(1, Mathf.Clamp01(currentFillAmount), 1);

            if (currentFillAmount > 1.0f)
            {
                Splash();
                Fail();
                gameActive = false;
            }
        }
    }

    void StopPouring()
    {
        isPouring = false;
        gameActive = false;

        // Success Check: Is the fill inside the green zone?
        // Note: You'll need to adjust these numbers based on your sprite sizes
        if (currentFillAmount >= 0.7f && currentFillAmount <= 0.85f)
        {
            Win();
        }
        else
        {
            Fail();
        }
    }

    void Splash()
    {
        if (splashEffect != null) splashEffect.Play();
    }
}