using UnityEngine;

public class LockUIFocus : MonoBehaviour
{
    // Adjust this number until the book fits the width of your screen perfectly
    public float targetWidth = 20f;

    void Update()
    {
        Camera cam = GetComponent<Camera>();
        // This math forces the camera's zoom (orthographicSize) 
        // to adapt to the screen's height-to-width ratio.
        cam.orthographicSize = (targetWidth / cam.aspect) / 2f;
    }
}
