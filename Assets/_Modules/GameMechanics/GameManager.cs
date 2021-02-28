using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;

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
        if (!SceneManager.GetActiveScene().name.Equals("Main Menu"))
        {
            if (GM != this)
                GM = this;

            EventSystem.current.onPlayerRangeEnter += OnPlayerRangeEnter;
            EventSystem.current.onPlayerRangeExit += OnPlayerRangeExit;
            EventSystem.current.onObjectRangeEnter += OnObjectRangeEnter;
            EventSystem.current.onObjectRangeExit += OnObjectRangeExit;
            EventSystem.current.onObjectInteract += OnObjectInteract;
            EventSystem.current.onObjectFinish += OnObjectFinish;
            EventSystem.current.onPlayerDeath += OnPlayerDeath;
        }
    }

    private void OnPlayerRangeEnter()
    {
        //Player entered
    }

    private void OnPlayerRangeExit()
    {
        //Player exit
    }

    private void OnObjectRangeEnter()
    {
        //Object entered
    }

    private void OnObjectRangeExit()
    {
        //Object exit
    }

    private void OnObjectInteract()
    {
        //Object interacted
    }

    private void OnObjectFinish()
    {
        //Object Finished
    }

    private void OnPlayerDeath()
    {
        //I die
    }
}
