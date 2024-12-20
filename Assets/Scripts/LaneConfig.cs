using UnityEngine;

[System.Serializable]
public class LaneConfig : MonoBehaviour
{
    public KeyCode keyToPress;
    public Transform spawnPoint;
    public GameObject notePrefab;
    public Transform targetPoint;
}