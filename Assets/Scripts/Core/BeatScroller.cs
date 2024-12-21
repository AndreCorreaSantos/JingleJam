using UnityEngine;

public class BeatScroller : MonoBehaviour
{
    [Header("Scroll Settings")]
    public float beatTempo;
    public bool hasStarted;
    public Vector3 scrollDirection = Vector3.down;

    private float scrollSpeed;

    void Start()
    {
        scrollSpeed = beatTempo / 60f;
        scrollDirection = scrollDirection.normalized;
    }

    void Update()
    {
        if (!hasStarted) return;
        
        foreach (Transform child in transform)
        {
            Vector3 movement = scrollDirection * scrollSpeed * Time.deltaTime;
            child.position += movement;
        }
    }

    public void SetTempo(float newTempo)
    {
        beatTempo = newTempo;
        scrollSpeed = beatTempo / 60f;
    }
}