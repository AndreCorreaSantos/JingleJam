using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance { get; private set; }

    [Header("Minigame Settings")]
    [SerializeField] private string minigameDataPath = "MinigameData/Active";
    [SerializeField] private List<MinigameData> availableMinigames;
    [SerializeField] private int numberOfMinigamesToPlay = 3;

    [SerializeField] private Animator transitionAnimator;
    [SerializeField] private float transitionTime = 1f;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadMinigameData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadMinigameData()
    {
        availableMinigames = new List<MinigameData>();

        MinigameData[] loadedData = Resources.LoadAll<MinigameData>(minigameDataPath);

        if (loadedData.Length == 0)
        {
            Debug.LogWarning($"No minigame data found in Resources/{minigameDataPath}");
            return;
        }

        availableMinigames.AddRange(loadedData);
        Debug.Log($"Loaded {availableMinigames.Count} minigames from Resources");
    }

    public void StartNewGame()
    {
        Debug.Log($"Starting New Game");
        AudioManager.Instance.PlaySound("Play");
        // Create MatchManager
        GameObject matchObj = new GameObject("MatchManager");
        matchObj.AddComponent<MatchManager>();

        List<MinigameData> selectedMinigames;
        selectedMinigames = SelectRandomMinigames();

        MatchManager.Instance.InitializeMatch(selectedMinigames);
    }

    private List<MinigameData> SelectRandomMinigames()
    {
        List<MinigameData> tempList = new List<MinigameData>(availableMinigames);
        List<MinigameData> selectedMinigames = new List<MinigameData>();

        for (int i = 0; i < numberOfMinigamesToPlay && tempList.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, tempList.Count);
            selectedMinigames.Add(tempList[randomIndex]);
            tempList.RemoveAt(randomIndex);
        }

        Debug.Log($"Selected {selectedMinigames.Count} minigames for this session");
        return selectedMinigames;
    }

    public void LoadMinigame(string minigameName, string minigameSceneName)
    {
        StartCoroutine(LoadScene(minigameSceneName));
        Debug.Log($"Loading minigame: {minigameName}");
    }

    public void LoadResults()
    {
        StartCoroutine(LoadScene("Results"));
        Debug.Log("All minigames completed, loading results");
    }

    public void ReturnToMainMenu()
    {
        StartCoroutine(LoadScene("MainMenuScene"));
        Debug.Log("Returning to main menu");
        Destroy(MatchManager.Instance);
    }

    public List<MinigameData> GetAvailableMinigames()
    {
        return availableMinigames;
    }

    public void PlaySpecificMinigame(MinigameData minigame)
    {
        // Create MatchManager
        GameObject matchObj = new GameObject("MatchManager");
        matchObj.AddComponent<MatchManager>();

        List<MinigameData> minigameList = new List<MinigameData>() { minigame };
        MatchManager.Instance.InitializeMatch(minigameList);
        Debug.Log($"Debug Mode: Loading specific minigame: {minigame.minigameName}");
    }


    private IEnumerator LoadScene(string name)
    {
        transitionAnimator.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(name);
        transitionAnimator.SetTrigger("End");
    }
}