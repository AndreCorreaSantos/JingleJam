using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ResultsCanvas : MonoBehaviour
{
    [Header("Results Display")]
    [SerializeField] private GameObject resultStatsPrefab;
    [SerializeField] private Transform resultsContainer;

    [SerializeField] private TextMeshProUGUI totalAccuracyText;
    [SerializeField] private TextMeshProUGUI totalScoreText;
    [SerializeField] private TextMeshProUGUI maxMultiplierText;

    [Header("Color Thresholds")]
    [SerializeField] private float goodThreshold = 80f;    // Green threshold
    [SerializeField] private float okayThreshold = 50f;    // Yellow threshold
    // Below okayThreshold will be red

    // Color definitions
    private readonly Color goodColor = Color.green;
    private readonly Color okayColor = Color.yellow;
    private readonly Color badColor = Color.red;

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

        int totalScore = 0;
        float totalAccuracy = 0;
        float maxMultiplier = 0;

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
                rankValue: result.rank,
                accuracyPercentage: result.percentageHit
            );

            totalScore += result.score;
            totalAccuracy += result.percentageHit;
            maxMultiplier = Mathf.Max(maxMultiplier, result.finalMultiplier);
        }

        float averageAccuracy = allResults.Count > 0 ? totalAccuracy / allResults.Count : 0;

        // Update UI texts with colors
        if (totalAccuracyText != null)
        {
            totalAccuracyText.text = $"{averageAccuracy:F1}%";
            totalAccuracyText.color = GetColorForValue(averageAccuracy);
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

    private Color GetColorForValue(float value)
    {
        if (value >= goodThreshold)
            return goodColor;
        else if (value >= okayThreshold)
            return okayColor;
        else
            return badColor;
    }
}