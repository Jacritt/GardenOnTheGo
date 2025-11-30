using UnityEngine;
using UnityEngine.SceneManagement;

public class GardenUIManager : MonoBehaviour
{
    private static GardenUIManager _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

