using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugMinigameMenu : MonoBehaviour
{
    [SerializeField] private GameObject debugPanel;
    [SerializeField] private TMP_Dropdown minigameDropdown;
    [SerializeField] private Button playSelectedButton;
    [SerializeField] private Toggle debugToggle;

    private void Start()
    {
        SetupDebugMenu();

        debugPanel.SetActive(false);

        if (debugToggle != null)
        {
            debugToggle.onValueChanged.AddListener(OnDebugToggleChanged);
        }
    }

    private void OnDebugToggleChanged(bool isOn)
    {
        debugPanel.SetActive(isOn);
    }

    private void SetupDebugMenu()
    {
        minigameDropdown.ClearOptions();

        var minigames = GameManager.Instance.GetAvailableMinigames();
        foreach (var minigame in minigames)
        {
            minigameDropdown.options.Add(new TMP_Dropdown.OptionData(minigame.minigameName));
        }

        minigameDropdown.RefreshShownValue();
        playSelectedButton.onClick.AddListener(PlaySelectedMinigame);
    }

    private void PlaySelectedMinigame()
    {
        var minigames = GameManager.Instance.GetAvailableMinigames();
        if (minigameDropdown.value < minigames.Count)
        {
            GameManager.Instance.PlaySpecificMinigame(minigames[minigameDropdown.value]);
        }
    }
}