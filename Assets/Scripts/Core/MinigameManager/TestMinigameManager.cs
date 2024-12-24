using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestMinigameManager : MinigameManager
{
    [Header("UI References")]
    [SerializeField] private Button completeButton;
    [SerializeField] private TextMeshProUGUI stateText;

    protected override void Start()
    {
        base.Start();

        completeButton.onClick.AddListener(() => CompleteMinigame());
        UpdateStateText();
    }

    protected override void StartMinigame()
    {
        base.StartMinigame();
        UpdateStateText();
        Debug.Log("Minigame Started!");
    }

    public override void CompleteMinigame()
    {
        base.CompleteMinigame();
        UpdateStateText();
        Debug.Log("Minigame Completed!");
    }

    private void UpdateStateText()
    {
        if (stateText != null)
        {
            stateText.text = $"State: {currentState}";
        }
    }
}