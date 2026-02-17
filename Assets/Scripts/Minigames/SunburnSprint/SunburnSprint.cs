using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SunburnSprint : MiniGame
{
    [System.Serializable]
    public class PlantData
    {
        public GameObject plantObject;
        public bool prefersSun; // Check this for sun-loving plants
        public float stressLevel = 0f; // Range 0 to 1
        public GameObject stressBar; // This should be the BarAnchor
    }

    [Header("Game Settings")]
    public List<PlantData> plants;
    public float stressIncreaseRate = 0.5f;
    public Transform sunZoneThreshold;

    public override void StartGame(float duration)
    {
        base.StartGame(duration);

        // We removed the random placement so your plants stay on the ground
        StartCoroutine(SurvivalCountdown(duration - 0.1f));
    }

    void Update()
    {
        if (!IsActive) return;

        foreach (var p in plants)
        {
            // 1. Logic Check: Is it in the sun?
            bool isInSun = p.plantObject.transform.position.x > sunZoneThreshold.position.x;

            // 2. Stress Calculation
            if (isInSun != p.prefersSun)
            {
                p.stressLevel += stressIncreaseRate * Time.deltaTime;
            }
            else
            {
                p.stressLevel = Mathf.Max(0, p.stressLevel - (stressIncreaseRate * Time.deltaTime));
            }

            // 3. Visual Feedback: Color Tinting
            SpriteRenderer renderer = p.plantObject.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                // Clamp stress between 0 and 1 for the Lerp function
                float colorT = Mathf.Clamp01(p.stressLevel);
                if (p.prefersSun) // Sunflower/Wheat
                    renderer.color = Color.Lerp(Color.white, new Color(0.5f, 0.5f, 1f), colorT);
                else // Mushrooms
                    renderer.color = Color.Lerp(Color.white, new Color(1f, 0.6f, 0.3f), colorT);
            }

            // 4. Visual Feedback: Stress Bar Scaling
            if (p.stressBar != null)
            {
                // We use Clamp01 to make sure the bar doesn't grow past the black container
                float visualFill = Mathf.Clamp01(p.stressLevel);
                p.stressBar.transform.localScale = new Vector3(visualFill, 1, 1);
            }

            // 5. Failure Condition: If any plant reaches 100% stress
            if (p.stressLevel >= 1.0f)
            {
                Fail();
            }
        }
    }

    IEnumerator SurvivalCountdown(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        // If we haven't failed by now, we win!
        if (IsActive) Win();
    }

    // REMOVED: void Start() - The GameManager now triggers the game!
}