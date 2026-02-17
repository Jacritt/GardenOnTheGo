using UnityEngine;

public class DraggableMatchingPlant : MonoBehaviour
{
    [Header("Match Settings")]
    public int plantID; // 0=Red, 1=Blue, 2=Yellow, 3=Green, 4=Purple

    private bool isDragging = false;
    private Vector3 offset;
    private Vector3 startPosition;

    [HideInInspector]
    public bool isPlaced = false;

    void Start()
    {
        // Stores the starting position so it can snap back if you miss the pot
        startPosition = transform.position;
    }

    void OnMouseDown()
    {
        if (isPlaced) return; // Don't move if already correctly potted

        isDragging = true;
        // Calculate the distance between the mouse and the object's center
        offset = transform.position - GetMouseWorldPos();
    }

    void OnMouseUp()
    {
        isDragging = false;
        // Start a Coroutine to wait one tiny moment before checking if we should snap back
        StartCoroutine(SnapBackCheck());
    }

    System.Collections.IEnumerator SnapBackCheck()
    {
        // Wait for the end of the physics frame so the PotTarget can finish its work
        yield return new WaitForFixedUpdate();

        // Now, if the pot didn't claim the plant, snap it back
        if (!isPlaced)
        {
            transform.position = startPosition;
        }
    }

    void Update()
    {
        if (isDragging && !isPlaced)
        {
            transform.position = GetMouseWorldPos() + offset;
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10; // Standard 2D camera distance
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}