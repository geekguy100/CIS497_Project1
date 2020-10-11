using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

/*
 * Sam Ferstein, Chris Smith, Kyle Grenier
 * Project 1
 * A UI Manager that handles displaying text and keeping track of the in-game timer.
 */

public class UIManager : Singleton<UIManager>
{
    [Header("Timer Assets")]
    [SerializeField] private float timer = 60f;
    public TextMeshProUGUI timerText;

    [Header("Scoring Assets")]
    public TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highscoreText;

    [Header("Tutorial Assets")]
    public TextMeshProUGUI movementTutorial;
    public TextMeshProUGUI grabbingTutorial;
    public TextMeshProUGUI goalTutorial;
    [SerializeField] private TextMeshProUGUI throwingTutorial;
    [SerializeField] private bool tutorial = false;
    public bool Tut { get { return tutorial; } }

    [Header("Game Status Text")]
    [SerializeField] private GameObject loseText;
    [SerializeField] private GameObject winText;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject sensitivityPanel;

    [Header("Fading Assets")]
    [SerializeField] private Image loadPanel;
    [SerializeField] private float fadeDuration = 5f;
    public bool finishedFading { get; private set; }

    [Header("Other")]
    [SerializeField] private Slider mouseSlider;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        
        mouseSlider.value = PlayerPrefs.GetFloat("Mouse Sensitivity");
        if (mouseSlider.value == 0)
            mouseSlider.value = mouseSlider.maxValue;

        //Make the loadPanel opaque.
        Color c = loadPanel.color;
        c.a = 1;
        loadPanel.color = c;

        UpdateScore(0, PlayerPrefs.GetInt("Highscore"));
        timerText.text = "Time: " + timer;

        if (tutorial)
        {
            movementTutorial.enabled = true;
            grabbingTutorial.enabled = false;
            throwingTutorial.enabled = false;
            goalTutorial.enabled = false;
            timerText.SetText("Time: INF");
            Tutorial();
        }
    }

    public void UpdateMouseSensitivity()
    {
        FindObjectOfType<CameraController>().sensitivity = mouseSlider.value;
        PlayerPrefs.SetFloat("Mouse Sensitivity", mouseSlider.value);
    }

    // Update is called once per frame
    void Update()
    {
        //Don't use timer in the tutorial.
        if (!tutorial && !GameManager.instance.GameOver && GameManager.instance.gameStarted)
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
        //Note that the game also ends when the player reaches the winning score, so make sure
        //the game is not already over.
        if (timer <= 0 && !GameManager.instance.GameOver)
        {
            GameManager.instance.GameOver = true;
        }
    }

    public void UpdateScore(int s, int hs)
    {
        scoreText.text = "Score: " + s + "/" + ScoreManager.instance.WinningScore;
        highscoreText.text = "Highscore: " + hs;
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
        throwingTutorial.enabled = true;

        yield return new WaitForSeconds(10);
        throwingTutorial.enabled = false;
        goalTutorial.enabled = true;

        yield return new WaitForSeconds(10);
        goalTutorial.enabled = false;
    }

    public void OnGameWin()
    {
        winText.SetActive(true);
    }

    public void OnGameLose()
    {
        loseText.SetActive(true);
    }

    public void PauseGame(bool paused)
    {
        if (paused)
        {
            pauseMenu.SetActive(true);
            sensitivityPanel.SetActive(true);
        }
        else
        {
            pauseMenu.SetActive(false);
            sensitivityPanel.SetActive(false);
        }
    }
}
