using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/*
 * Sam Ferstein
 * Project 1
 * This is the UI Manager that will control the timer as well as the win/loss screens?
 */

public class UIManager : MonoBehaviour
{
    public float timer = 60;
    //score won't actually be a float
    public float score = 0;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    //public GameManager gameManager;
    //public ScoreManager scoreManager;
    public bool gameActive = true;

    // Start is called before the first frame update
    void Start()
    {
        //gameManager = gameObject.GetComponent<GameManager>();
        //scoreManager = gameObject.GetComponent<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameActive == true)
        {
            timer -= Time.deltaTime;
            score += Time.deltaTime;
            timerText.text = "Time: " + timer.ToString("f0");
            scoreText.text = "Score: " + score.ToString("f0");
            if (timer <= 0)
            {
                gameActive = false;
                timerText.text = "Game Over!\nPress R to Restart.";
                //gameManager.GameOver = true;
            }
            if(score >= 20)
            {
                gameActive = false;
                timerText.text = "You Won!\nPress R to Restart.";
            }
        }
        if (gameActive == false && Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Restarting...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
