using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitBook : MonoBehaviour
{
    public string mainSceneName = "Main";

    // This MUST say 'public' or the button can't find it!
    public void CloseBotanyBook()
    {
        SceneManager.LoadScene(mainSceneName);
    }
}