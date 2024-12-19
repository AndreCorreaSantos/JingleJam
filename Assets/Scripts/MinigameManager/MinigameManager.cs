using UnityEngine;
using System;

public abstract class MinigameManager : MonoBehaviour
{
    protected MinigameData minigameData;

    protected enum MinigameState
    {
        Ready,      // Initial state
        Playing,    // Game is active
        Completed,  // Game is finished
        Failed      // Player failed the game
    }

    protected MinigameState currentState = MinigameState.Ready;
    protected float score = 0;

    // Events
    public event Action OnMinigameCompleted;

    protected virtual void Start()
    {
        minigameData = GameManager.Instance.GetCurrentMinigameData();
        if (minigameData == null)
        {
            Debug.LogError("No minigame data found!");
            return;
        }
        PrepareMinigame();
    }

    protected virtual void PrepareMinigame()
    {
        currentState = MinigameState.Ready;
        StartMinigame();
    }

    protected virtual void StartMinigame()
    {
        currentState = MinigameState.Playing;
    }

    protected virtual void CompleteMinigame()
    {
        if (currentState != MinigameState.Playing) return;

        currentState = MinigameState.Completed;
        OnMinigameCompleted?.Invoke();

        GameManager.Instance.LoadNextMinigame();
    }
}