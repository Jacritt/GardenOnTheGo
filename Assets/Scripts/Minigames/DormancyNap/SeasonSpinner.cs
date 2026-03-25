using UnityEngine;
using System.Collections;

public class SeasonSpinner : MonoBehaviour
{
    public enum Season { Winter, Spring, Summer, Fall }

    [Header("Current Game State")]
    public Season currentSeason;
    public bool isSpinning = false;

    [Header("Spin Settings")]
    public float spinDuration = 2.0f; // How long the animation takes
    public int extraSpins = 3;       // How many full circles before stopping

    // The Enum order is: 0:Winter, 1:Spring, 2:Summer, 3:Fall
    // We map your custom degrees to those specific slots:
    private float[] seasonAngles = {
    315f, // Index 0: Winter (You said 315)
    135f, // Index 1: Spring (You said 135)
    45f,  // Index 2: Summer (You said 45)
    225f  // Index 3: Fall   (You said 225)
};
    public void StartRandomSpin()
    {
        if (!isSpinning)
        {
            // Pick a random season index (0 to 3)
            int randomIndex = Random.Range(0, 4);
            currentSeason = (Season)randomIndex;

            StartCoroutine(AnimateSpin(seasonAngles[randomIndex]));
        }
    }

    IEnumerator AnimateSpin(float targetAngle)
    {
        isSpinning = true;
        float elapsed = 0f;

        // We use the current Z rotation as the starting point
        float startAngle = transform.eulerAngles.z;

        // This ensures the arrow always spins FORWARD (clockwise or counter, depending on preference)
        // Adding (extraSpins * 360) makes it look like a real game show wheel
        float totalRotation = (extraSpins * 360f) + 360f;
        float endAngle = startAngle - totalRotation + (targetAngle - startAngle);

        while (elapsed < spinDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / spinDuration;

            // Easing: Starts fast, slows down at the end
            float curvedT = t * t * (3f - 2f * t);

            float currentZ = Mathf.Lerp(startAngle, endAngle, curvedT);
            transform.eulerAngles = new Vector3(0, 0, currentZ);

            yield return null;
        }

        // Final Snap to your custom degree
        transform.eulerAngles = new Vector3(0, 0, targetAngle);
        isSpinning = false;
    }
   
}
