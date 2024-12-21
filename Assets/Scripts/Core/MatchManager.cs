using UnityEngine;
using System.Collections.Generic;

public class MatchManager : MonoBehaviour
{
    // Singleton instance
    public static MatchManager Instance { get; private set; }

    private List<MinigameData> matchMinigames;
    private Dictionary<string, float> minigameScores;
    private int currentMinigameIndex;

    // Match scores
    private float currentMultiplier;

    // Match statistics
    // private float totalScore;
    // private float bestScore;
    // private string bestMinigame;
    // private float averageScore;

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("Found existing MatchManager, destroying this one");
            Destroy(gameObject);
        }
    }

    public void InitializeMatch(List<MinigameData> selectedMinigames)
    {
        matchMinigames = new List<MinigameData>(selectedMinigames);
        minigameScores = new Dictionary<string, float>();
        currentMinigameIndex = 0;
        // totalScore = 0;
        // bestScore = 0;
        Debug.Log($"Match initialized with {matchMinigames.Count} minigames");

        GameManager.Instance.LoadMinigame(selectedMinigames[currentMinigameIndex].minigameName, selectedMinigames[currentMinigameIndex].sceneName);
        currentMinigameIndex += 1;
    }

    public void NextMinigame()
    {
        Debug.Log($"Current Minigame Index: {currentMinigameIndex} | Match Count: {matchMinigames.Count}");
        if (currentMinigameIndex < matchMinigames.Count)
        {
            GameManager.Instance.LoadMinigame(matchMinigames[currentMinigameIndex].minigameName, matchMinigames[currentMinigameIndex].sceneName);
            currentMinigameIndex += 1;
        }
        else
        {
            GameManager.Instance.LoadResults(); // Game Ended
        }
    }

    public float GetCurrentMultiplier()
    {
        return currentMultiplier;
    }

    public void SetCurrentMultiplier(float newMultiplier)
    {
        currentMultiplier = newMultiplier;
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}