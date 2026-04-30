using UnityEngine;
using System.Collections.Generic;

public class WaterOrWreckIt : MiniGame
{
    [System.Serializable]
    public class PotType
    {
        public string name;
        public GameObject potVisual;
        public float fillSpeed;
        public Vector2 targetRange;
    }

    [Header("Pot Configurations")]
    public List<PotType> potOptions;

    [Header("Water Elements")]
    public GameObject waterFill;
    public GameObject waterStream;
    public ParticleSystem splashEffect;

    private PotType currentPot;
    private float currentFillAmount = 0f;
    private bool isPouring = false;
    private bool gameActive = false;

    // This is called by the GameManager when the minigame starts
    public override void StartGame(float duration)
    {
        base.StartGame(duration);
        SetupRandomPot();
        gameActive = true;
    }

    void SetupRandomPot()
    {
        // Pick a random pot from your list
        currentPot = potOptions[Random.Range(0, potOptions.Count)];

        // Toggle visuals
        foreach (var p in potOptions) p.potVisual.SetActive(false);
        currentPot.potVisual.SetActive(true);

        // Set water to your exact "Floor" position and width
        waterFill.transform.position = new Vector3(-0.82f, -4.3765f, 0f);

        currentFillAmount = 0;
        waterFill.transform.localScale = new Vector3(6.802f, 0, 1);
    }

    void Update()
    {
        // Only allow logic if the game is actually running
        if (!IsActive || !gameActive) return;

        if (Input.GetMouseButtonDown(0)) isPouring = true;
        if (Input.GetMouseButtonUp(0)) StopPouring();

        if (isPouring)
        {
            currentFillAmount += currentPot.fillSpeed * Time.deltaTime;
            waterFill.transform.localScale = new Vector3(6.802f, currentFillAmount, 1);

            // Safety check to prevent infinite filling
            if (currentFillAmount > 5.0f)
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

        // Final Range Check
        if (currentFillAmount >= currentPot.targetRange.x && currentFillAmount <= currentPot.targetRange.y)
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