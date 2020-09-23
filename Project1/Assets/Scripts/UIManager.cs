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

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private float timer = 60f;
    //score won't actually be a float
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreNeededText;
    //public GameManager gameManager;
    //public ScoreManager scoreManager;

    // Start is called before the first frame update
    void Start()
    {
        //gameManager = gameObject.GetComponent<GameManager>();
        //scoreManager = gameObject.GetComponent<ScoreManager>();
        scoreNeededText.text = "Score Needed: " + ScoreManager.instance.winningScore;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.GameOver)
            CountdownTimer();
    }

    void CountdownTimer()
    {
        timer -= Time.deltaTime;
        //score += Time.deltaTime;
        timerText.text = "Time: " + timer.ToString("f0");
        //scoreText.text = "Score: " + score.ToString("f0");

        //Make sure the GameManager knows the timer is over.
        if (timer <= 0)
        {
            GameManager.instance.GameOver = true;
            //timerText.text = "Game Over!\nPress R to Restart.";
            //gameManager.GameOver = true;
        }
    }

    public void UpdateScore(int s)
    {
        scoreText.text = "Score: " + s;
    }
}