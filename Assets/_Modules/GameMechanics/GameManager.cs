using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;
    public GameObject player;
    public GameObject playerSpawn;
    public GameObject enemy;
    public GameObject enemySpawn;
    public CanvasGroup endScreen;
    public CanvasGroup deathScreen;
    //public GameObject pauseScreen;
    public GameObject door;
    public GameObject key;
    public GameObject lockedWall;

    private bool isPaused = false;
    private bool endEntered = false;

    private const float END_ANIM_DUR = 5f;

    private const float DEATH_ANIM_DUR = 2.0F;
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
        EventSystem.current.onKeyEnterTrigger += OnKeyEnterTrigger;
        EventSystem.current.onPlayerCaughtTrigger += OnPlayerCaughtTrigger;
        EventSystem.current.onRestrainOrderCheck += OnRestrainOrderCheck;
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
    private void OnPlayerCaughtTrigger()
    {
        player.transform.position = playerSpawn.transform.position;
        player.transform.rotation = Quaternion.identity;
        player.transform.GetComponent<NavMeshAgent>().Warp(playerSpawn.transform.position);

        enemy.transform.GetComponent<NavMeshAgent>().Warp(enemySpawn.transform.position);
        enemy.GetComponent<Enemy_AI>().Reset();
        enemy.GetComponent<VoidToggledEnemy>().Reset();

        StartCoroutine(UIFade.FadeCanvas(deathScreen, 1f, 1f, DEATH_ANIM_DUR, 0f));
        Invoke(nameof(DisableCharacterMovement), 0f);

        StartCoroutine(UIFade.FadeCanvas(deathScreen, 1f, 0f, 1f, DEATH_ANIM_DUR));
        Invoke(nameof(EnableCharacterMovement), DEATH_ANIM_DUR);

    }

    private void OnRestrainOrderCheck()
    {
        VoidController controller = player.GetComponent<VoidController>();
        controller.Repress();
        controller.Reveal(lockedWall);
        lockedWall.SetActive(false);
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
    
    public void OnKeyEnterTrigger()
    {
        player.GetComponent<InteractionManager>().currentInteraction = null;
        player.GetComponent<InteractionManager>().interacting = false;

        key.SetActive(false); //Gameobject.Destroy(key);
        door.GetComponent<intr_Door>().LockOpen();
    }
}
