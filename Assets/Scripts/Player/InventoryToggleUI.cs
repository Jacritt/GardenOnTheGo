using UnityEngine;

public class InventoryToggleUI : MonoBehaviour
{
    public void ToggleInventory()
    {
        if (PlantSelectionUI.Instance.panel.activeSelf)
        {
            PlantSelectionUI.Instance.Close();
        }
        else
        {
            // Open in VIEW MODE (no plot passed)
            PlantSelectionUI.Instance.Open(null);
        }
    }
}
