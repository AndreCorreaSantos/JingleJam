using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MinigameResultsStats : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI accuracy;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI rank;

    [Header("Rank Colors")]
    private readonly Color sRankColor = new Color(1f, 0.7f, 0f);  // Gold
    private readonly Color aRankColor = new Color(0.8f, 0.8f, 0.8f);  // Silver
    private readonly Color bRankColor = new Color(0.8f, 0.5f, 0.2f);  // Bronze
    private readonly Color cRankColor = Color.red;
    private readonly Color defaultRankColor = Color.white;

    public void UpdateResults(string titleText, string accuracyValue, string scoreValue, string rankValue, float accuracyPercentage)
    {
        title.text = titleText;
        accuracy.text = accuracyValue;
        score.text = scoreValue;
        rank.text = rankValue;

        // Set accuracy color based on percentage
        accuracy.color = GetAccuracyColor(accuracyPercentage);

        // Set rank color based on rank value
        rank.color = GetRankColor(rankValue);
    }

    private Color GetAccuracyColor(float accuracyPercentage)
    {
        if (accuracyPercentage >= 80f)
            return Color.green;
        else if (accuracyPercentage >= 50f)
            return Color.yellow;
        else
            return Color.red;
    }

    private Color GetRankColor(string rankValue)
    {
        return rankValue.ToUpper() switch
        {
            "S" => sRankColor,
            "A" => aRankColor,
            "B" => bRankColor,
            "C" => cRankColor,
            _ => defaultRankColor
        };
    }
}