using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;
    public GameObject player;
    public CanvasGroup endScreen;
    //public GameObject pauseScreen;
    public GameObject door;

    private bool isPaused = false;
    private bool endEntered = false;

    private const float END_ANIM_DUR = 3f;
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
        if (!endEntered)
        {
            endEntered = true;
            StartCoroutine(UIFade.FadeCanvas(endScreen, 0f, 1f, END_ANIM_DUR, 0f));
            Invoke(nameof(DisableCharacterMovement), END_ANIM_DUR);
        }
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

    private void EnableCharacterMovement()
    {
        player.GetComponent<BaseMotor>().SetMovementLock(false);
        player.GetComponent<BaseMotor>().SetRotationLock(false);
    }

    private void DisableCharacterMovement()
    {
        player.GetComponent<BaseMotor>().SetMovementLock(true);
        player.GetComponent<BaseMotor>().SetRotationLock(true);
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
