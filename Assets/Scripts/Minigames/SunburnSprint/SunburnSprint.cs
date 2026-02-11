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
        public float stressLevel = 0f;
        public GameObject stressBar; // Visual feedback
    }

    public List<PlantData> plants;
    public float stressIncreaseRate = 0.5f;
    public Transform sunZoneThreshold; // An X-position that divides sun and shade

    public override void StartGame(float duration)
    {
        base.StartGame(duration);

        // Randomly place plants at the start
        foreach (var p in plants)
        {
            float randomX = Random.Range(-5f, 5f);
            p.plantObject.transform.localPosition = new Vector3(randomX, 0, 0);
        }

        StartCoroutine(SurvivalCountdown(duration - 0.1f));
    }

    void Update()
    {
        if (!IsActive) return;

        foreach (var p in plants)
        {
            bool isInSun = p.plantObject.transform.position.x > sunZoneThreshold.position.x;

            // STRESS LOGIC
            if (isInSun != p.prefersSun)
            {
                p.stressLevel += stressIncreaseRate * Time.deltaTime;
            }
            else
            {
                p.stressLevel = Mathf.Max(0, p.stressLevel - (stressIncreaseRate * Time.deltaTime));
            }

            // VISUAL FEEDBACK: COLOR TINTING
            SpriteRenderer renderer = p.plantObject.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                if (p.prefersSun) // Sunflower/Wheat
                {
                    // Fade from White to Blue (Cold/Shade stress)
                    renderer.color = Color.Lerp(Color.white, new Color(0.5f, 0.5f, 1f), p.stressLevel);
                }
                else // Mushrooms
                {
                    // Fade from White to a "Crispy" Brown/Orange (Sunburn stress)
                    renderer.color = Color.Lerp(Color.white, new Color(1f, 0.6f, 0.3f), p.stressLevel);
                }
            }

            /// Instead of looking for a SpriteRenderer, we just look for the Transform
            if (p.stressBar != null)
            {
                // We scale the X from 0 (empty) to 1 (full)
                p.stressBar.transform.localScale = new Vector3(p.stressLevel, 1, 1);
            }
        }
    }

    IEnumerator SurvivalCountdown(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (IsActive) Win();
    }
    void Start()
    {
        StartGame(15f); // Tests the game with a 15-second timer
    }
}