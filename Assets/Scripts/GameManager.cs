using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance { get; private set; }

    [Header("Minigame Settings")]
    [SerializeField] private string minigameDataPath = "MinigameData/Active";
    [SerializeField] private List<MinigameData> availableMinigames;
    [SerializeField] private int numberOfMinigamesToPlay = 3;

    // Game progress
    private List<MinigameData> selectedMinigames;
    private int currentMinigameIndex = 0;
    protected MinigameData currentMinigameData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            selectedMinigames = new List<MinigameData>();
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
        selectedMinigames.Clear();
        currentMinigameIndex = 0;

        SelectRandomMinigames();
        LoadNextMinigame();
    }

    private void SelectRandomMinigames()
    {
        List<MinigameData> tempList = new List<MinigameData>(availableMinigames);

        for (int i = 0; i < numberOfMinigamesToPlay && tempList.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, tempList.Count);
            selectedMinigames.Add(tempList[randomIndex]);
            tempList.RemoveAt(randomIndex);
        }

        Debug.Log($"Selected {selectedMinigames.Count} minigames for this session");
    }

    public void LoadNextMinigame()
    {
        if (currentMinigameIndex < selectedMinigames.Count)
        {
            currentMinigameData = selectedMinigames[currentMinigameIndex];

            string nextScene = selectedMinigames[currentMinigameIndex].sceneName;
            SceneManager.LoadScene(nextScene);
            Debug.Log($"Loading minigame: {selectedMinigames[currentMinigameIndex].minigameName}");
            currentMinigameIndex++;
        }
        else
        {
            LoadResults();
        }
    }

    private void LoadResults()
    {
        // SceneManager.LoadScene("Results");
        // Debug.Log("All minigames completed, loading results");

        Debug.LogWarning("No Results Scene, loading Menu");
        ReturnToMainMenu();
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
        Debug.Log("Returning to main menu");
    }

    public MinigameData GetCurrentMinigameData()
    {
        return currentMinigameData;
    }

    public List<MinigameData> GetAvailableMinigames()
    {
        return availableMinigames;
    }

    public void PlaySpecificMinigame(MinigameData minigame)
    {
        currentMinigameData = minigame;
        SceneManager.LoadScene(minigame.sceneName);
        Debug.Log($"Debug Mode: Loading specific minigame: {minigame.minigameName}");
    }

}