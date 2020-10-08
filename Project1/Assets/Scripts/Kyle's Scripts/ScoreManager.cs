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
    private int winningScore = 10;
    public int WinningScore
    {
        get
        {
            return winningScore;
        }
    }

    //Total wins of session.
    private int totalWins = 0;
    public int TotalWins
    {
        get { return totalWins; }
        set
        {
            if (value != totalWins)
            {
                totalWins = value;
                PlayerPrefs.SetInt("Total Wins", totalWins);
            }
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
            if (GameManager.instance.GameOver || !GameManager.instance.gameStarted)
                return;

            score = value;
            
            //Only increase the highscore if in an actual game, not the tutorial
            if (score > highScore && !UIManager.instance.Tut)
                highScore = score;

            UIManager.instance.UpdateScore(score, highScore);
            if (score >= winningScore && !GameManager.instance.GameWon)
            {
                TotalWins++;
                GameManager.instance.GameWon = true;
            }
        }
    }

    private void Start()
    {
        highScore = PlayerPrefs.GetInt("Highscore");
        Debug.Log("Current Highscore: " + highScore);

        //If the tutorial is active, make the winning score 10.
        if (UIManager.instance.Tut)
        {
            winningScore = 10;
            return;
        }

        totalWins = PlayerPrefs.GetInt("Total Wins");
        CalculateWinningScore();
    }

    public void GameOver()
    {
        //If the player reached a new highscore, update it in PlayerPrefs.
        if (highScore > PlayerPrefs.GetInt("Highscore"))
            PlayerPrefs.SetInt("Highscore", highScore);
    }

    //Initial score is 10. 
    //2 more toys are added to the pool on each consecutive win.
    private void CalculateWinningScore()
    {
        winningScore = 10 + (2 * totalWins);
        Debug.Log("Winning score: 10 + (2 * " + totalWins + ") = " + winningScore);
    }
}