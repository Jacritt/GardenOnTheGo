using UnityEngine;

public class Mole : MonoBehaviour
{
    public int moleIndex; // Set this to 0 for Mole1, 1 for Mole2, etc. in the Inspector
    private WhackAMoleGame gameController;

    void Start()
    {
        gameController = GetComponentInParent<WhackAMoleGame>();
    }

    void OnMouseDown()
    {
        gameController.WhackMole(moleIndex);
    }
}
