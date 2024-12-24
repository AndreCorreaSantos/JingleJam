using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MinigameResultsStats : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI accuracy;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI rank;


    public void UpdateResults(string titleText, string accuracyValue, string scoreValue, string rankValue)
    {
        title.text = titleText;
        accuracy.text = accuracyValue;
        score.text = scoreValue;
        rank.text = rankValue;


        // foreach (MinigameResults result in allResults)
        // {
        //     MinigameResultsStats statsDisplay = Instantiate(resultStatsPrefab, resultsContainer);

        //     statsDisplay.UpdateResults(
        //         titleText: result.minigameName,
        //         accuracyValue: $"{result.percentageHit:F1}%",
        //         scoreValue: result.score.ToString("N0"),
        //         rankValue: result.rank
        //     );

        //     totalScore += result.score;
        // }
    }

}
