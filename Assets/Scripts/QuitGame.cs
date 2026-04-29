using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public GameObject quitPanel;

    // Called when Quit button is pressed
    public void AskToQuit()
    {
        quitPanel.SetActive(true);
    }

    // Called when No is pressed
    public void CancelQuit()
    {
        quitPanel.SetActive(false);
    }

    // Called when Yes is pressed
    public void ConfirmQuit()
    {
        Debug.Log("Closing Game...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}