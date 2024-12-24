using UnityEngine;
using System.Collections.Generic;
using System;

public class MatchManager : MonoBehaviour
{
    // Singleton instance
    public static MatchManager Instance { get; private set; }

    private List<MinigameData> matchMinigames;
    private Dictionary<string, float> minigameScores;
    private int currentMinigameIndex;

    // Match scores
    private float currentMultiplier;
    public event Action<float> OnMultiplierChanged;

    public MinigameManager minigameManager;

    private List<MinigameResults> minigameResults = new List<MinigameResults>();

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
        currentMultiplier = 1;
        Debug.Log($"Match initialized with {matchMinigames.Count} minigames");

        GameManager.Instance.LoadMinigame(selectedMinigames[currentMinigameIndex].minigameName, selectedMinigames[currentMinigameIndex].sceneName);
    }

    public void NextMinigame()
    {
        currentMinigameIndex += 1;
        Debug.Log($"Current Minigame Index: {currentMinigameIndex} | Match Count: {matchMinigames.Count}");
        Destroy(minigameManager);
        if (currentMinigameIndex < matchMinigames.Count)
        {
            GameManager.Instance.LoadMinigame(matchMinigames[currentMinigameIndex].minigameName, matchMinigames[currentMinigameIndex].sceneName);
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
        if (currentMultiplier != newMultiplier)
        {
            currentMultiplier = newMultiplier;
            OnMultiplierChanged?.Invoke(currentMultiplier);
        }
    }

    public void SetMinigameManager(MinigameManager newMinigameManager)
    {
        minigameManager = newMinigameManager;
    }

    // Add this method to store minigame results
    public void StoreMinigameResults(MinigameResults results)
    {
        minigameResults.Add(results);
        Debug.Log($"Stored results for {results.minigameName}: Score={results.score}, Rank={results.rank}");
    }

    // Add method to get all results
    public List<MinigameResults> GetAllResults()
    {
        return minigameResults;
    }

    public MinigameData GetCurrentMinigameData()
    {
        return matchMinigames[currentMinigameIndex];
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}