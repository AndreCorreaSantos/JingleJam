using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugMenu : MonoBehaviour
{
    public static DebugMenu Instance { get; private set; }

    [SerializeField] private Toggle debugToggle;
    [SerializeField] private GameObject debugContent;

    // Play selected Minigame
    [SerializeField] private TMP_Dropdown minigameDropdown;
    [SerializeField] private Button playSelectedButton;

    // Minigame actions
    [SerializeField] private Button nextMinigameButton;
    [SerializeField] private Button increaseMultiplierButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetupDebugMenu();

        debugContent.SetActive(false);

        if (debugToggle != null)
        {
            debugToggle.onValueChanged.AddListener(OnDebugToggleChanged);
        }
    }

    private void OnDebugToggleChanged(bool isOn)
    {
        debugContent.SetActive(isOn);
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
        nextMinigameButton.onClick.AddListener(NextMinigame);
        increaseMultiplierButton.onClick.AddListener(IncreaseMultiplier);
    }

    private void PlaySelectedMinigame()
    {
        var minigames = GameManager.Instance.GetAvailableMinigames();
        if (minigameDropdown.value < minigames.Count)
        {
            GameManager.Instance.PlaySpecificMinigame(minigames[minigameDropdown.value]);
        }
    }

    private void NextMinigame()
    {
        if (MatchManager.Instance != null)
        {
            MatchManager.Instance.NextMinigame();
        }
    }

    private void IncreaseMultiplier()
    {
        if (MatchManager.Instance != null)
        {
            float currentMultiplier = MatchManager.Instance.GetCurrentMultiplier();
            MatchManager.Instance.SetCurrentMultiplier(Mathf.Min(currentMultiplier + .5f, 4f));
        }
    }
}