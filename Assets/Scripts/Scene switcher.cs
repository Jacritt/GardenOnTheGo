using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sceneswitcher : MonoBehaviour
{
    public static int selectedLevel;
    public int level;

    public void LoadScene(string sceneName)
    {
        selectedLevel = level;
        PlantSelectionUI.Instance.HideCanvas();
        SceneManager.LoadScene(sceneName);
    }
}
