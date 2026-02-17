using UnityEngine;

public class PotTarget : MonoBehaviour
{
    [Header("Match Settings")]
    public int potID; // 0=Red, 1=Blue, 2=Yellow, 3=Green, 4=Purple

    [HideInInspector]
    public bool isOccupied = false;

    // This checks the plant while it is hovering over the pot
    void OnTriggerStay2D(Collider2D other)
    {
        // Only trigger if the player lets go of the mouse button
        if (!Input.GetMouseButton(0) && !isOccupied)
        {
            // Try to find the DraggableMatchingPlant script on the object we touched
            DraggableMatchingPlant plant = other.GetComponent<DraggableMatchingPlant>();

            if (plant != null)
            {
                // CHECK THE ID MATCH
                if (plant.plantID == potID)
                {
                    // SUCCESS!
                    plant.isPlaced = true;
                    isOccupied = true;

                    // Snap the plant perfectly to the center of the pot
                    plant.transform.position = transform.position + new Vector3(0, 0.5f, 0);

                    Debug.Log("Match! Pot " + potID + " is now full.");
                }
            }
        }
    }
}