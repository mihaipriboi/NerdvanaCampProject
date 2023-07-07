using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject startScreen;
    [SerializeField] GameObject playScreen;

    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite emptyHeart;

    [SerializeField] GameObject[] hearts;

    public bool gamePaused;
    public bool loseLife;

    private int lives;

    void Start()
    {
        Application.targetFrameRate = 120;

        StopGame();

        lives = hearts.Length;

        for (int i = 0; i < lives; i++)
        {
            hearts[i].GetComponent<SpriteRenderer>().sprite = fullHeart;
        }
    }

    void Update()
    {
        if (loseLife)
        {
            loseLife = false;
            Die();
        }
    }

    public void StartGame()
    {
        gamePaused = false;

        player.GetComponent<Rigidbody2D>().isKinematic = false;

        startScreen.SetActive(false);
    }

    public void StopGame()
    {
        gamePaused = true;

        player.GetComponent<Rigidbody2D>().isKinematic = true;
    }

    public void Die()
    {
        lives--;
        hearts[lives].GetComponent<SpriteRenderer>().sprite = emptyHeart;
    }
}
