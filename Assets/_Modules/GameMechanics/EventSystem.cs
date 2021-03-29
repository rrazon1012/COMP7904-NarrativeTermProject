using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    public static EventSystem current;

    public void Awake()
    {
        current = this;
    }

    public event Action onPlayerRangeEnter;
    public void PlayerRangeEnter()
    {
        onPlayerRangeEnter?.Invoke();
    }

    public event Action onPlayerRangeExit;
    public void PlayerRangeExit()
    {
        onPlayerRangeExit?.Invoke();
    }

    public event Action onObjectRangeEnter;
    public void ObjectRangeEnter()
    {
        onPlayerRangeEnter?.Invoke();
    }

    public event Action onObjectRangeExit;
    public void ObjectRangeExit()
    {
        onObjectRangeExit?.Invoke();
    }

    public event Action onPlayerDeath;
    public void PlayerDeath()
    {
        onPlayerDeath?.Invoke();
    }

    public event Action onLockOpen;
    public void LockOpen()
    {
        onLockOpen?.Invoke();
    }

    public event Action onKeyEnterTrigger;
    public void KeyEnterTrigger()
    {
        onKeyEnterTrigger?.Invoke();
    }
    public event Action onPlayerCaughtTrigger;
    public void PlayerCaughtTrigger()
    {
        onPlayerCaughtTrigger?.Invoke();
    }

    public event Action onRestrainOrderCheck;
    public void RestrainOrderCheck()
    {
        onRestrainOrderCheck?.Invoke();
    }

    public event Action onSpawnEnemy;
    public void SpawnEnemy()
    {
        onSpawnEnemy?.Invoke();

    }
}
