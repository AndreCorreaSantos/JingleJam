using UnityEngine;

public class BeatScroller : MonoBehaviour
{
    [Header("Scroll Settings")]
    public float beatTempo;
    public bool hasStarted; 

    private float scrollSpeed;

    void Start()
    {
        scrollSpeed = beatTempo / 60f;
    }

    void Update()
    {
        if (!hasStarted)
        {
            return;
        }
        
        foreach (Transform child in transform)
        {
            Vector3 movement = new Vector3(0f, -scrollSpeed * Time.deltaTime, 0f);
            child.position += movement;
        }
    }

    public void SetTempo(float newTempo)
    {
        beatTempo = newTempo;
        scrollSpeed = beatTempo / 60f;
    }
}