using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] string sceneName;

    [SerializeField] GameObject player;

    [SerializeField] GameObject startScreen;
    [SerializeField] GameObject playScreen;
    [SerializeField] GameObject pauseScreen;

    [SerializeField] AudioSource audioSource;

    [SerializeField] GameObject[] fullHearts;
    [SerializeField] GameObject[] emptyHearts;

    [SerializeField] GameObject dashReady;
    [SerializeField] GameObject dashNotReady;

    [SerializeField] TMP_Text ScoreText;

    [SerializeField] TMP_Text HeartNo;

    public bool gamePaused;

    public bool loseLife;
    public bool useDash;
    public bool dashRdy;

    private int score;
    private int lives;

    void Start()
    {
        Application.targetFrameRate = 120;

        startScreen.SetActive(true);
        playScreen.SetActive(false);
        pauseScreen.SetActive(false);

        Time.timeScale = 0;
        audioSource.volume = 0.1f;

        lives = fullHearts.Length;
        HeartNo.text = "x " + lives;

        for (int i = 0; i < lives; i++)
        {
            fullHearts[i].SetActive(true);
            emptyHearts[i].SetActive(false);
        }

        dashReady.SetActive(true);
        dashNotReady.SetActive(false);

        ResetScore();
    }

    void Update()
    {
        if (loseLife)
        {
            loseLife = false;
            Die();
        }

        if (useDash)
        {
            useDash = false;
            UseDash();
        }
        if (dashRdy)
        {
            dashRdy = false;
            DashReady();
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (!gamePaused)
                StopGame();
            else
                StartGame();
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1;

        gamePaused = false;

        startScreen.SetActive(false);
        playScreen.SetActive(true);
        pauseScreen.SetActive(false);

        audioSource.volume = 0.2f;
    }

    public void StopGame()
    {
        Time.timeScale = 0;

        gamePaused = true;

        startScreen.SetActive(false);
        playScreen.SetActive(false);
        pauseScreen.SetActive(true);

        audioSource.volume = 0.05f;
    }

    public void Die()
    {
        lives--;
        HeartNo.text = "x " + lives;
        fullHearts[lives].SetActive(false);
        emptyHearts[lives].SetActive(true);
    }

    public void UseDash()
    {
        dashReady.SetActive(false);
        dashNotReady.SetActive(true);
    }
    public void DashReady()
    {
        dashReady.SetActive(true);
        dashNotReady.SetActive(false);
    }

    public void AddScore(int sc)
    {
        score += sc;
        ScoreText.text = "Score: " + score;
    }

    public void ResetScore()
    {
        score = 0;
        ScoreText.text = "Score: " + score;
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
