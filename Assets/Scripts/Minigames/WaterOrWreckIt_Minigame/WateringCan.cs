using UnityEngine;

public class WateringCan : MonoBehaviour
{
    [Header("Movement Settings")]
    public float followSpeed = 10f;
    public float tiltAngle = -45f; // Negative usually tilts the spout down
    public float tiltSpeed = 10f;

    [Header("Water Stream")]
    public GameObject waterStream; // Drag your Water_Stream child object here

    void Start()
    {
        // Ensure the stream is off when the game starts
        if (waterStream != null)
            waterStream.SetActive(false);
    }

    void Update()
    {
        // 1. Smoothly follow the mouse
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // Keep it on the 2D plane
        transform.position = Vector3.Lerp(transform.position, mousePos, Time.deltaTime * followSpeed);

        // 2. Handle Pouring Logic
        if (Input.GetMouseButton(0)) // While holding Left Click
        {
            StartPouring();
            // Adds a slight flicker to the width to simulate moving water
            float jiggle = Mathf.PingPong(Time.time * 20f, 0.05f);
            waterStream.transform.localScale = new Vector3(0.1f + jiggle, 5.0f, 1f);
        }
        else // When mouse is released
        {
            StopPouring();
        }
    }

    void StartPouring()
    {
        // Tilt the can forward
        Quaternion targetRotation = Quaternion.Euler(0, 0, tiltAngle);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * tiltSpeed);

        // Turn on the stream
        if (waterStream != null)
            waterStream.SetActive(true);
    }

    void StopPouring()
    {
        // Tilt the can back to level (upright)
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, Time.deltaTime * tiltSpeed);

        // Turn off the stream
        if (waterStream != null)
            waterStream.SetActive(false);
    }
}