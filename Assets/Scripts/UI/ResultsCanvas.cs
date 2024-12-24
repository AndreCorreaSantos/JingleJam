using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ResultsCanvas : MonoBehaviour
{

    [Header("Results Display")]
    [SerializeField] private GameObject resultStatsPrefab; // Prefab of the stats UI
    [SerializeField] private Transform resultsContainer; // Parent transform to hold the stats

    [SerializeField] private TextMeshProUGUI totalAccuracyText;
    [SerializeField] private TextMeshProUGUI totalScoreText;
    [SerializeField] private TextMeshProUGUI maxMultiplierText;



    private void Start()
    {
        UpdateResults();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.ReturnToMainMenu();
        }
    }

    private void UpdateResults()
    {
        List<MinigameResults> allResults = MatchManager.Instance.GetAllResults();

        // Initialize totals
        int totalScore = 0;
        float totalAccuracy = 0;
        float maxMultiplier = 0;

        // Clear any existing results first
        foreach (Transform child in resultsContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (MinigameResults result in allResults)
        {
            GameObject statsDisplay = Instantiate(resultStatsPrefab, resultsContainer);
            MinigameResultsStats statsDisplayScript = statsDisplay.GetComponent<MinigameResultsStats>();

            statsDisplayScript.UpdateResults(
                titleText: result.minigameName,
                accuracyValue: $"{result.percentageHit:F1}%",
                scoreValue: result.score.ToString("N0"),
                rankValue: result.rank
            );

            // Update totals
            totalScore += result.score;
            totalAccuracy += result.percentageHit;
            maxMultiplier = Mathf.Max(maxMultiplier, result.finalMultiplier);
        }

        // Calculate average accuracy
        float averageAccuracy = allResults.Count > 0 ? totalAccuracy / allResults.Count : 0;

        // Update UI texts
        if (totalAccuracyText != null)
        {
            totalAccuracyText.text = $"{averageAccuracy:F1}%";
        }

        if (totalScoreText != null)
        {
            totalScoreText.text = $"{totalScore:N0}";
        }

        if (maxMultiplierText != null)
        {
            maxMultiplierText.text = $"x{maxMultiplier:F1}";
        }

    }
}
