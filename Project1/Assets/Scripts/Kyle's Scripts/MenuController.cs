/*****************************************************************************
// File Name :         MenuController.cs
// Author :            Kyle Grenier
// Creation Date :     October 01, 2020
// Assignment:         Project 2 - CIS 497
// Brief Description : Enables use of UI buttons to navigate scenes.
*****************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Image loadPanel;
    [SerializeField] private float fadeDuration = 3.2f;

    private void Start()
    {
        loadPanel.gameObject.SetActive(false);
    }

    //Starts the coroutine to fade to black and load the scene with given name.
    public void LoadScene(string name)
    {
        StartCoroutine(FadeAndLoadScene(name));
    }

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        loadPanel.gameObject.SetActive(true);

        //Create a new color and make it completely opaque.
        Color c = loadPanel.color;

        Color finishedColor = c;
        finishedColor.a = 1;

        loadPanel.color = finishedColor;
        //Fade to transparent over time fadeDurationation.
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            c.a = Mathf.Lerp(0, 1, normalizedTime * normalizedTime);
            loadPanel.color = c;
            yield return null;
        }

        loadPanel.color = finishedColor;
        SceneManager.LoadScene(sceneName);
    }
}