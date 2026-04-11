using UnityEngine;
using UnityEngine.SceneManagement;

public class PageFlipper : MonoBehaviour
{
    [Header("Navigation Settings")]
    public string targetPageName; // Type "BotanyBook2" or "BotanyBook" here

    public void FlipPage()
    {
        // Optional: You could add a sound effect trigger here later!
        Debug.Log("Flipping to: " + targetPageName);
        SceneManager.LoadScene(targetPageName);
    }
}