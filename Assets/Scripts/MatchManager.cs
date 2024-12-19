using UnityEngine;
using System.Collections.Generic;

public class MatchManager : MonoBehaviour
{
    // Singleton instance
    public static MatchManager Instance { get; private set; }

    private List<MinigameData> matchMinigames;
    private Dictionary<string, float> minigameScores;
    private int currentMinigameIndex;

    // Match statistics
    private float totalScore;
    private float bestScore;
    private string bestMinigame;
    private float averageScore;

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
        totalScore = 0;
        bestScore = 0;
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

    public void RecordMinigameScore(string minigameName, float score)
    {
        minigameScores[minigameName] = score;
        totalScore += score;

        // Update best score
        if (score > bestScore)
        {
            bestScore = score;
            bestMinigame = minigameName;
        }

        // Update average
        averageScore = totalScore / minigameScores.Count;

        Debug.Log($"Recorded score for {minigameName}: {score}");
    }

    public Dictionary<string, float> GetMatchResults()
    {
        return new Dictionary<string, float>
        {
            { "totalScore", totalScore },
            { "bestScore", bestScore },
            { "averageScore", averageScore }
        };
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}