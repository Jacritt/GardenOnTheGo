using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Gameplay")]
    public List<GameObject> miniGamePrefabs; // assign prefab scenes or prefabs of mini-games
    public Transform miniGameSpawnParent; // where the mini-game instance will be placed in hierarchy
    public float startRoundDuration = 4f;
    public float minRoundDuration = 1.2f;
    public float difficultyRampPerScore = 0.05f; // how much faster per score

    [Header("Player")]
    public int startingLives = 3;
    public int lives;
    public int score;

    [Header("UI")]
    public Text scoreText;
    public GameObject[] lifeIcons; // set size = max lives, toggle active/inactive
    public GameObject gameOverPanel;
    public Text finalScoreText;

    bool isPlaying = false;
    Coroutine cycleCoroutine;
    GameObject currentMiniGame;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        lives = startingLives;
        score = 0;
        UpdateUI();
        StartGameLoop();
    }

    public void StartGameLoop()
    {
        if (cycleCoroutine != null) StopCoroutine(cycleCoroutine);
        isPlaying = true;
        gameOverPanel.SetActive(false);
        cycleCoroutine = StartCoroutine(MiniGameCycle());
    }

    IEnumerator MiniGameCycle()
    {
        while (isPlaying)
        {
            float roundDuration = Mathf.Max(minRoundDuration, startRoundDuration - (score * difficultyRampPerScore));
            // pick a random mini-game prefab
            int idx = Random.Range(0, miniGamePrefabs.Count);
            if (currentMiniGame != null) Destroy(currentMiniGame);
            currentMiniGame = Instantiate(miniGamePrefabs[idx], miniGameSpawnParent);
            // try to find a MiniGame component and call StartGame if present
            MiniGame mg = currentMiniGame.GetComponent<MiniGame>();
            if (mg != null) mg.StartGame(roundDuration);

            // wait for either roundDuration or the mini-game to end early (mini-game should call WinGame/LoseLife)
            float timer = 0f;
            while (timer < roundDuration && currentMiniGame != null)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            // if the mini-game didn't call back, treat as failure
            if (currentMiniGame != null)
            {
                // ask the mini-game if it's still active
                MiniGame still = currentMiniGame.GetComponent<MiniGame>();
                if (still != null && still.IsActive)
                {
                    // time up -> failure
                    LoseLife();
                    Destroy(currentMiniGame);
                    currentMiniGame = null;
                }
            }

            // short delay between rounds
            yield return new WaitForSeconds(0.35f);
        }
    }

    public void WinGame()
    {
        score++;
        UpdateUI();
        // optionally play SFX etc
        if (currentMiniGame != null) Destroy(currentMiniGame);
        currentMiniGame = null;
        // next round will be spawned by loop
    }

    public void LoseLife()
    {
        lives--;
        UpdateUI();
        if (currentMiniGame != null) Destroy(currentMiniGame);
        currentMiniGame = null;
        if (lives <= 0)
        {
            GameOver();
        }
    }

    void UpdateUI()
    {
        if (scoreText != null) scoreText.text = $"Score: {score}";
        for (int i = 0; i < lifeIcons.Length; i++)
        {
            lifeIcons[i].SetActive(i < lives);
        }
    }

    void GameOver()
    {
        isPlaying = false;
        if (cycleCoroutine != null) StopCoroutine(cycleCoroutine);
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            if (finalScoreText != null) finalScoreText.text = $"Final Score: {score}";
        }
    }

    // optional: restart from UI button
    public void RestartGame()
    {
        // reset and reload current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Main");
    }
}
