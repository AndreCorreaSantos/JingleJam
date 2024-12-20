using UnityEngine;

public class MenuCanvas : MonoBehaviour
{
    public void StartNewGame()
    {
        GameManager.Instance.StartNewGame();
    }
}
