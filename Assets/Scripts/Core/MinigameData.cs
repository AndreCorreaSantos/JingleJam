using UnityEngine;

[CreateAssetMenu(fileName = "MinigameData", menuName = "Scriptable Objects/MinigameData")]
public class MinigameData : ScriptableObject
{
    [Header("Basic Info")]
    public string minigameName;
    public string sceneName;
}