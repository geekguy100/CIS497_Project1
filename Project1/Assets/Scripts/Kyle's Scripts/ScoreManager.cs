/*****************************************************************************
// File Name :         ScoreManager.cs
// Author :            Kyle Grenier
// Creation Date :     September 21, 2020
//
// Brief Description : Script to handle accumulating and managing the score.
*****************************************************************************/
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    //The score the player must achieve in set time to win.
    [SerializeField] private int winningScore = 20;

    //The high score of the player.
    private int highScore;

    private int score = 0;
    public int Score
    {
        get { return score; }
        set    
        {
            score = value;
            //TODO: Update UI. UI Manager??
            Debug.Log("Score: " + score);
            if (score >= winningScore)
                GameManager.instance.GameWon = true;
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