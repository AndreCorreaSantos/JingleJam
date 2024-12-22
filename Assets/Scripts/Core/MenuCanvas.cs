using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuCanvas : MonoBehaviour
{
    [Header("Menu Buttons")]
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button optionsButton;


    private void Start()
    {
        // Add button listeners
        startGameButton.onClick.AddListener(StartNewGame);
    }

    public void StartNewGame()
    {
        GameManager.Instance.StartNewGame();
    }

    public void OpenOptions()
    {
        // Implement options menu opening logic
        Debug.Log("Opening Options Menu");
    }

}