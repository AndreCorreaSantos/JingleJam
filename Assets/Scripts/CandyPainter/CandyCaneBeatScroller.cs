using UnityEngine;

public class CandyCaneBeatScroller : BeatScroller
{
    [Header("Candy Settings")]
    [SerializeField] private float beatDropDistance = 0.5f;
    
    private float lastDropBeat;

    private void OnEnable()
    {
        lastDropBeat = 0;
    }

    private void Update()
    {
        if (!hasStarted || Conductor.instance == null) return;

        float currentBeat = Conductor.instance.songPositionInBeats;
        
        // Move on beat
        if (currentBeat >= lastDropBeat + 1)
        {
            MoveOnBeat();
            lastDropBeat = Mathf.Floor(currentBeat);
        }
    }

    private void MoveOnBeat()
    {
        foreach (Transform child in transform)
        {
            Vector3 newPos = child.position;
            newPos.y -= beatDropDistance;
            child.position = newPos;
            
            if (newPos.y < -5f)
            {
                Destroy(child.gameObject);
            }
        }
    }
}