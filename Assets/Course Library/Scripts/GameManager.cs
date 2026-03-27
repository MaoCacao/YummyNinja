using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<GameObject> targets;
    private float spawnRate = 1.0f;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI gameOverText;
    private int score;
    public int lives;
    public bool isGameActive;
    public Button restartButton;
    public GameObject titleScreen;
    public enum GameMode { Easy, Medium, Hard };
    public GameMode gameMode;
    public GameObject pausePanel;
    public bool isPaused = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGameActive)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            pausePanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            pausePanel.SetActive(false);
        }
    }

    public void StartGame(int difficulty)
    {
        SetGameMode(difficulty);
        isGameActive = true;
        spawnRate /= difficulty;
        score = 0;
        StartCoroutine(SpawnTarget());
        UpdateScore(0);
        if (gameMode != GameMode.Medium)
        {
            UpdateLives(3);
        }
        else
        {
            livesText.text = "Lives: ∞";
            lives = 1;
        }
        titleScreen.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    public void UpdateScore(int currentScore)
    {
        score += currentScore;
        scoreText.text = "Score: " + score;
    }

    public void UpdateLives(int currentLives)
    {
        lives = currentLives;
        livesText.text = "Lives: " + lives;
    }

    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        isGameActive = false;
        restartButton.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetGameMode(int difficulty)
    {
        switch (difficulty)
        {
            case 1:
                gameMode = GameMode.Easy;
                break;
            case 2:
                gameMode = GameMode.Medium;
                break;
            default:
                gameMode = GameMode.Hard;
                break;
        }
    }

    IEnumerator SpawnTarget()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targets.Count);
            Instantiate(targets[index]);
        }
    }
}
