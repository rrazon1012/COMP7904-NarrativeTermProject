using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;
    //public GameObject pauseScreen;
    public GameObject door;

    private bool isPaused = false;

    private void Awake()
    {
        if (GM == null)
        {
            DontDestroyOnLoad(gameObject);
            GM = this;
        }
    }

    private void Start()
    {
        if (GM != this)
            GM = this;

        EventSystem.current.onPlayerRangeEnter += OnPlayerRangeEnter;
        EventSystem.current.onPlayerRangeExit += OnPlayerRangeExit;
        EventSystem.current.onObjectRangeEnter += OnObjectRangeEnter;
        EventSystem.current.onObjectRangeExit += OnObjectRangeExit;
        EventSystem.current.onPlayerDeath += OnPlayerDeath;
    }

    private void OnPlayerRangeEnter()
    {
        Debug.Log("Player range enter");
    }

    private void OnPlayerRangeExit()
    {
        Debug.Log("Player range exit");
    }

    private void OnObjectRangeEnter()
    {
        Debug.Log("Object range enter");
    }

    private void OnObjectRangeExit()
    {
        Debug.Log("Object range exit");
    }

    private void OnPlayerDeath()
    {
        Debug.Log("I die");
    }

    private void PauseTime()
    {
        Time.timeScale = 0;
    }

    private void ResumeTime()
    {
        Time.timeScale = 1;
    }

    public void PauseGame()
    {
        isPaused = true;
        PauseTime();
        //pauseScreen.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
