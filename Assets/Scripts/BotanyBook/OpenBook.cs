using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenBook : MonoBehaviour
{
    [Header("Settings")]
    public string botanyBookSceneName = "BotanyBook"; // Must match your scene file name exactly!

    public void EnterBotanyBook()
    {
        Debug.Log("Opening Botany Book...");
        SceneManager.LoadScene(botanyBookSceneName);
    }
}