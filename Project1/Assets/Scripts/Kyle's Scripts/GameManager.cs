/*****************************************************************************
// File Name :         GameManager.cs
// Author :            Kyle Grenier
// Creation Date :     September 21, 2020
//
// Brief Description : Script to manage the state of the game.
*****************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    //The time the player has to complete the level (in seconds).
    [SerializeField] private float playTime = 60f;

    private bool gameOver = false;
    private bool gameWon = false;

    public bool GameOver
    {
        get { return gameOver; }
        //Set the gameOver value.
        //If gameOver and the player didn't win, 
        set
        {
            gameOver = value;
            if (gameOver && !gameWon)
            {
                Debug.Log("You lose!");
                ScoreManager.instance.GameOver();
                //TODO: Update UI. UI Manager??
            }
            else if (gameWon)
            {
                Debug.Log("You win!");
                ScoreManager.instance.GameOver();
                //TODO: Update UI. UI Manager??
            }
            else //If GameWon was set to false and not GameOver, make sure gameOver is still updated accordingly.
                gameOver = false;
        }
    }
    public bool GameWon
    {
        //Set the gameWon variable.
        //If the game has been won, make sure GameOver is set to true
        //so the appropriate mechanics take place.
        set
        {
            gameWon = value;
            GameOver = value;
            if (!gameWon)
                Debug.LogWarning("Please don't set GameWon to false by itself; do so my modifying GameOver.");
        }
    }

    //TODO: Code to start the game, such as starting the timer, spawning toys, etc.
    //Perhabs this should run on scene load? Or maybe when they press a UI button, but the former seems more practical.
    //Before the timer actually starts, there should be a 3 second cooldown before actual gameplay begins, so the players don't instantly spawn into
    //the scene and have to start.
    public void StartGame()
    {

    }

    private void Update()
    {
        //TODO: Implement a restart feature upon winning or losing the game.
        //Do we want the game to go back to the main menu, a level selection screen, or just restart the current scene?
        //For now, I made it restart the current scene.
        if (gameOver && Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Restarting...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (!gameOver)
        {
            //DEBUG: Increase score on key press.
            if (Input.GetKeyUp(KeyCode.P))
                ScoreManager.instance.Score++;
        }
    }
}