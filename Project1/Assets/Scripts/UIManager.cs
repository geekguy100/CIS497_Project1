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
    [SerializeField] private float timer = 60f;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI movementTutorial;
    public TextMeshProUGUI grabbingTutorial;
    public TextMeshProUGUI throwingTutorial;
    public TextMeshProUGUI goalTutorial;

    [SerializeField] private Image loadPanel;
    [SerializeField] private float fadeDuration = 5f;
    [SerializeField] private bool tutorial = false;
    public bool finishedFading { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
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
            throwingTutorial.enabled = false;
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
    public void FadePanel()
    {
        StartCoroutine(FadeLoadPanel());
    }

    private IEnumerator FadeLoadPanel()
    {
        finishedFading = false;

        //Create a new color and make it completely opaque.
        Color c = loadPanel.color;

        Color finishedColor = c;
        finishedColor.a = 0;


        //Fade to transparent over time fadeDuration.
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            c.a = Mathf.Lerp(1, 0, normalizedTime * normalizedTime);
            loadPanel.color = c;
            yield return null;
        }

        loadPanel.color = finishedColor;
        finishedFading = true;
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
        throwingTutorial.enabled = true;

        yield return new WaitForSeconds(10);
        throwingTutorial.enabled = false;
        goalTutorial.enabled = true;

        yield return new WaitForSeconds(10);
        goalTutorial.enabled = false;
    }
}