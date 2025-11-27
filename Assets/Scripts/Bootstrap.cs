using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Bootstrap : MonoBehaviour
{
    // Put the persistent ui scene in Build Settings before Main
    // (or use the exact name below)
    public string persistentScene = "PersistentUI";
    public string mainScene = "Main";

    private IEnumerator Start()
    {
        // If PersistentUI already loaded, skip
        if (!SceneManager.GetSceneByName(persistentScene).isLoaded)
            yield return SceneManager.LoadSceneAsync(persistentScene, LoadSceneMode.Additive);

        // Now load main scene normally (single or additive depending on your desired unload)
        yield return SceneManager.LoadSceneAsync(mainScene, LoadSceneMode.Single);
    }
}

