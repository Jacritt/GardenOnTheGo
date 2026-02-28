using UnityEngine;
using System.Collections.Generic;

public class WaterOrWreckIt : MiniGame
{
    void Start()
    {
        // This starts the game immediately for testing without the GameManager
        StartGame(10f);
    }

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
        currentPot = potOptions[Random.Range(0, potOptions.Count)];

        foreach (var p in potOptions) p.potVisual.SetActive(false);
        currentPot.potVisual.SetActive(true);

        // This line tells the script: "Find the green zone inside the pot I just turned on"
        targetZone = currentPot.potVisual.GetComponentInChildren<SpriteRenderer>().gameObject;

        currentFillAmount = 0;
        waterFill.transform.localScale = new Vector3(1, 0, 1);
    }

    void Update()
    {
        if (!IsActive) return;

        if (Input.GetMouseButtonDown(0))
        {
            isPouring = true;
            Debug.Log("Pouring Started!");
        }

        if (Input.GetMouseButtonUp(0))
        {
            isPouring = false;
            Debug.Log("Pouring Stopped!");
            StopPouring();
        }

        if (isPouring)
        {
            currentFillAmount += currentPot.fillSpeed * Time.deltaTime;

            // This is the line that actually moves the blue square
            if (waterFill != null)
            {
                waterFill.transform.localScale = new Vector3(1, currentFillAmount, 1);
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