using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

/*
 * Sam Ferstein
 * Project 1
 * This is the UI Manager that will control the timer as well as the win/loss screens?
 */

public class UIManager : Singleton<UIManager>
{
    [Header("Timer Assets")]
    [SerializeField] private float timer = 60f;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;

    [Header("Tutorial Assets")]
    public TextMeshProUGUI movementTutorial;
    public TextMeshProUGUI grabbingTutorial;
    public TextMeshProUGUI goalTutorial;
    [SerializeField] private bool tutorial = false;

    [Header("Game Status Text")]
    [SerializeField] private GameObject loseText;
    [SerializeField] private GameObject winText;
    [SerializeField] private GameObject pauseMenu;

    [Header("Fading Assets")]
    [SerializeField] private Image loadPanel;
    [SerializeField] private float fadeDuration = 5f;
    public bool finishedFading { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.gameStarted = false;
        loseScreen.SetActive(false);
        winScreen.SetActive(false);
        tutorialScreen.SetActive(true);
        tutorial = true;

        //Make the loadPanel opaque.
        Color c = loadPanel.color;
        c.a = 1;
        loadPanel.color = c;

        UpdateScore(0);
        timerText.text = "Time: " + timer;

        if (tutorial)
        {
            movementTutorial.enabled = true;
            grabbingTutorial.enabled = false;
            goalTutorial.enabled = false;
            Tutorial();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.GameOver && GameManager.instance.gameStarted)
            CountdownTimer();
    }

    //Made a public function to avoid calling StartCoroutine() in the GameManager.
    //Makes things look cleaner.
    //Reverse = true if panel should fade from transparent to opaque.
    //If fadeDur = 0, set fade duration to UIManager's fadeDuration.
    public void FadePanel(bool reverse = false, float fadeDur = 0f)
    {
        if (fadeDur == 0)
            fadeDur = fadeDuration;

        StartCoroutine(FadeLoadPanel(reverse, fadeDur));
    }

    private IEnumerator FadeLoadPanel(bool reverse, float fadeDur)
    {
        loadPanel.gameObject.SetActive(true);
        finishedFading = false;

        //Create a new color.
        Color c = loadPanel.color;
        int finishedAlpha = 0;
        int startingAlpha = 1;

        if (reverse)
        {
            finishedAlpha = 1;
            startingAlpha = 0;
        }

        //Fade to transparent over time fadeDuration.
        for (float t = 0; t < fadeDur; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDur;
            c.a = Mathf.Lerp(startingAlpha, finishedAlpha, normalizedTime * normalizedTime);
            loadPanel.color = c;
            yield return null;
        }

        finishedFading = true;
        loadPanel.gameObject.SetActive(false);
    }

    void CountdownTimer()
    {
        timer -= Time.deltaTime;
        timerText.text = "Time: " + timer.ToString("f0");

        //Make sure the GameManager knows the timer is over, ending the game.
        if (timer <= 0)
        {
            GameManager.instance.GameOver = true;
        }
    }

    public void UpdateScore(int s)
    {
        scoreText.text = "Score: " + s + "/" + ScoreManager.instance.WinningScore;
    }

    public void Tutorial()
    {
        StartCoroutine(TutorialMessage());
    }

    private IEnumerator TutorialMessage()
    {
        yield return new WaitForSeconds(10);
        movementTutorial.enabled = false;
        grabbingTutorial.enabled = true;

        yield return new WaitForSeconds(10);
        grabbingTutorial.enabled = false;
        goalTutorial.enabled = true;

        yield return new WaitForSeconds(10);
        goalTutorial.enabled = false;
        tutorialScreen.SetActive(false);
        GameManager.instance.gameStarted = true;
    }

    public void OnGameWin()
    {
        winScreen.SetActive(true);
    }

    public void OnGameLose()
    {
        loseScreen.SetActive(true);
    }

    public void PauseGame(bool paused)
    {
        if (paused)
            pauseMenu.SetActive(true);
        else
            pauseMenu.SetActive(false);
    }
}
