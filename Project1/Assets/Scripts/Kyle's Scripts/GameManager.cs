/*****************************************************************************
// File Name :         GameManager.cs
// Author :            Kyle Grenier
// Creation Date :     September 21, 2020
//
// Brief Description : Script to manage the state of the game.
*****************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    private bool gameOver = false;
    private bool gameWon = false;
    public bool gameStarted { get; private set; }

    private AudioSource gameAudio;
    public AudioClip loseSFX;
    public AudioClip winSFX;

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
                gameAudio.PlayOneShot(loseSFX, 1.0f);
                ScoreManager.instance.GameOver();
                UIManager.instance.OnGameLose();
            }
            else if (gameOver && gameWon)
            {
                Debug.Log("You win!");
                gameAudio.PlayOneShot(winSFX, 1.0f);
                ScoreManager.instance.GameOver();
                UIManager.instance.OnGameWin();
            }
            else //If GameWon was set to false and not GameOver, make sure gameOver is still updated accordingly.
            {
                gameWon = value;
            }

        }
    }
    public bool GameWon
    {
        get { return gameWon; }

        //Set the gameWon variable.
        //If the game has been won, make sure GameOver is set to true
        //so the appropriate mechanics take place.
        set
        {
            gameWon = value;

            //GameOver = value; 
            //^^^ Setting this would also make the game end if the game has been won,
            //but we want the player to keep striving for a high score even after they picked up the 
            //winning amount of toys.
            if (gameWon)
                Debug.Log("Player has reached a winnable score.");
            else //if gameWon is false, reset GameOver as well.
            {
                GameOver = value;
                Debug.LogWarning("Please don't set GameWon to false by itself; do so my modifying GameOver.");
            }
        }
    }

    //TODO: Code to start the game, such as starting the timer, spawning toys, etc.
    //Perhabs this should run on scene load? Or maybe when they press a UI button, but the former seems more practical.
    //Before the timer actually starts, there should be a 3 second cooldown before actual gameplay begins, so the players don't instantly spawn into
    //the scene and have to start.
    public void Start()
    {
        //On scene load, first spawn toys, make game screen visible, then start timer by making gameStarted = true.
        StartCoroutine("StartGame");
    }

    private IEnumerator StartGame()
    {
        gameStarted = false;
        ToySpawner.instance.SpawnToys();
        UIManager.instance.FadePanel();
        gameAudio = GetComponent<AudioSource>();

        //Wait until the UIManager has finished fading the load panel, check toys in meantime.
        while (!UIManager.instance.finishedFading)
        {
            ToySpawner.instance.CheckToys();
            yield return null;
        }
            

        
        gameStarted = true;
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