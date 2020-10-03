/*****************************************************************************
// File Name :         ScoreManager.cs
// Author :            Kyle Grenier
// Creation Date :     September 21, 2020
// Assignment:         Project 2 - CIS 497
// Brief Description : Script to handle accumulating and managing the score.
*****************************************************************************/
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    //The score the player must achieve in set time to win.
    private int winningScore = 20;
    public int WinningScore
    {
        get
        {
            return winningScore;
        }
    }


    //The high score of the player.
    private int highScore;

    private int score = 0;
    public int Score
    {
        get { return score; }
        set    
        {
            if (GameManager.instance.GameOver)
                return;

            score = value;
            UIManager.instance.UpdateScore(score);
            if (score >= winningScore && !GameManager.instance.GameWon)
                GameManager.instance.GameWon = true;

            if (score > highScore)
                Debug.Log("Player has a new highscore of " + score);
        }
    }

    private void Start()
    {
        highScore = PlayerPrefs.GetInt("Highscore");
        Debug.Log("Current Highscore: " + highScore);
    }

    public void GameOver()
    {
        if (score > highScore)
        {
            PlayerPrefs.SetInt("Highscore", score);
            Debug.Log("Congrats! You achieved a new high score of " + score);
        }
    }
}