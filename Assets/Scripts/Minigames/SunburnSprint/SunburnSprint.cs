using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SunburnSprint : MiniGame
{
    [System.Serializable]
    public class PlantData
    {
        public GameObject plantObject;
        public bool prefersSun;
        public float stressLevel = 0f;
        public GameObject stressBar;
    }

    [Header("Game Settings")]
    public List<PlantData> plants;

    // We removed the fixed 0.5 and will calculate this when the game starts
    private float dynamicStressRate;

    public Transform sunZoneThreshold;

    public override void StartGame(float duration)
    {
        base.StartGame(duration);

        // SYNC LOGIC: Calculate rate so the bar hits 1.0 exactly when time runs out
        // We use duration - 0.2f to give the player a tiny bit of breathing room
        dynamicStressRate = 1f / (duration - 0.2f);

        StartCoroutine(SurvivalCountdown(duration - 0.1f));
    }

    void Update()
    {
        if (!IsActive) return;

        foreach (var p in plants)
        {
            bool isInSun = p.plantObject.transform.position.x > sunZoneThreshold.position.x;

            // STRESS LOGIC using the dynamic rate
            if (isInSun != p.prefersSun)
            {
                p.stressLevel += dynamicStressRate * Time.deltaTime;
            }
            else
            {
                // Recovery can stay fast or use the same rate
                p.stressLevel = Mathf.Max(0, p.stressLevel - (dynamicStressRate * Time.deltaTime));
            }

            // Visual Feedback: Color Tinting
            SpriteRenderer renderer = p.plantObject.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                float colorT = Mathf.Clamp01(p.stressLevel);
                if (p.prefersSun)
                    renderer.color = Color.Lerp(Color.white, new Color(0.5f, 0.5f, 1f), colorT);
                else
                    renderer.color = Color.Lerp(Color.white, new Color(1f, 0.6f, 0.3f), colorT);
            }

            // Visual Feedback: Stress Bar Scaling
            if (p.stressBar != null)
            {
                float visualFill = Mathf.Clamp01(p.stressLevel);
                p.stressBar.transform.localScale = new Vector3(visualFill, 1, 1);
            }

            // Failure Condition
            if (p.stressLevel >= 1.0f)
            {
                Fail();
            }
        }
    }

    IEnumerator SurvivalCountdown(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (IsActive) Win();
    }
}