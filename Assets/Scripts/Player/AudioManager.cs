using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Music")]
    public AudioSource musicSource;
    public AudioClip loginMusic;
    public AudioClip navigationMusic;
    public AudioClip minigameMusic;

    [Header("SFX")]
    public AudioSource sfxSource;
    public AudioClip buttonClickSound;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // THIS makes it persist
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        PlayLoginMusic();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Change music based on scene name
        if (scene.name == "Login")
        {
            PlayLoginMusic();
        }
        else if (scene.name == "MainGameScene")
        {
            PlayMinigameMusic();
        }
        else
        {
            PlayNavigationMusic();
        }
    }

    public void PlayLoginMusic()
    {
        PlayMusic(loginMusic);
    }

    public void PlayNavigationMusic()
    {
        PlayMusic(navigationMusic);
    }

    public void PlayMinigameMusic()
    {
        PlayMusic(minigameMusic);
    }

    void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip) return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayButtonClick()
    {
        sfxSource.PlayOneShot(buttonClickSound);
    }
}
