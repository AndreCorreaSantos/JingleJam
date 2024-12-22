using UnityEngine;

public class RhythmSpawner : MonoBehaviour 
{
    [Header("Lane Configuration")]
    [SerializeField] private LaneConfig[] lanes;
    
    [Header("References")]
    [SerializeField] private BeatScroller beatScroller;
    
    [Header("Pattern Settings")]
    [SerializeField] private float baseSpawnChance = 0.7f;
    [SerializeField] private float minNoteInterval = 0.5f;
    [Range(0f, 1f)]
    [SerializeField] private float offbeatChance = 0.5f;

    [Header("Burst Settings")]
    [SerializeField] private float burstChance = 0.2f;
    [SerializeField] private int maxBurstNotes = 3;
    [SerializeField] private float burstNoteInterval = 0.25f;
    
    [Header("Dynamic Difficulty")]
    [SerializeField] private float volumeThreshold = 0.05f;
    [SerializeField] private float maxSpawnChance = 0.9f;
    [SerializeField] private float minSpawnChance = 0.5f;
    
    private float nextPossibleSpawnBeat;
    private float[] lastSpawnedBeatPerLane;
    private System.Random random;
    private bool inBurst;
    private int burstNotesLeft;

    void Start()
    {
        random = new System.Random();
        nextPossibleSpawnBeat = 0f;
        
        if (beatScroller == null)
        {
            Debug.LogError("RhythmSpawner: BeatScroller not found!");
            enabled = false;
            return;
        }

        if (lanes.Length > 0 && lanes[0].spawnPoint != null && lanes[0].targetPoint != null)
        {
            Vector3 direction = (lanes[0].targetPoint.position - lanes[0].spawnPoint.position).normalized;
            beatScroller.scrollDirection = direction;
        }
        
        lastSpawnedBeatPerLane = new float[lanes.Length];
        for (int i = 0; i < lastSpawnedBeatPerLane.Length; i++)
        {
            lastSpawnedBeatPerLane[i] = -1f;
        }
    }

    void Update()
    {
        if (!Conductor.instance.hasStarted) return;

        float currentVolume = Conductor.instance.currentBeatVolume;
        if (currentVolume > volumeThreshold)
        {
            float volumeFactor = Mathf.Clamp01(currentVolume / volumeThreshold);
            baseSpawnChance = Mathf.Lerp(minSpawnChance, maxSpawnChance, volumeFactor);
            minNoteInterval = Mathf.Lerp(0.5f, 0.25f, volumeFactor);
        }

        float currentBeat = Conductor.instance.songPositionInBeats;
        
        if (currentBeat < nextPossibleSpawnBeat) return;

        float beatDecimal = currentBeat - Mathf.Floor(currentBeat);
        bool isOnBeat = beatDecimal < 0.05f || beatDecimal > 0.95f;
        bool isOffBeat = Mathf.Abs(beatDecimal - 0.5f) < 0.05f;

        if (!inBurst && isOnBeat && random.NextDouble() < burstChance)
        {
            inBurst = true;
            burstNotesLeft = random.Next(2, maxBurstNotes + 1);
        }

        if (inBurst)
        {
            if (burstNotesLeft > 0)
            {
                int laneIndex = random.Next(lanes.Length);
                if (currentBeat - lastSpawnedBeatPerLane[laneIndex] >= burstNoteInterval)
                {
                    SpawnNote(laneIndex);
                    burstNotesLeft--;
                    lastSpawnedBeatPerLane[laneIndex] = currentBeat;
                    nextPossibleSpawnBeat = currentBeat + burstNoteInterval;
                }
            }
            else
            {
                inBurst = false;
                nextPossibleSpawnBeat = currentBeat + minNoteInterval;
            }
        }
        else if ((isOnBeat && ShouldSpawnNote(baseSpawnChance)) || 
                 (isOffBeat && ShouldSpawnNote(offbeatChance)))
        {
            int laneIndex = random.Next(lanes.Length);
            
            if (currentBeat - lastSpawnedBeatPerLane[laneIndex] >= minNoteInterval)
            {
                SpawnNote(laneIndex);
                lastSpawnedBeatPerLane[laneIndex] = currentBeat;
                nextPossibleSpawnBeat = currentBeat + (minNoteInterval * 0.25f);
            }
        }
    }

    private bool ShouldSpawnNote(float chance)
    {
        return random.NextDouble() < chance;
    }

    private void SpawnNote(int laneIndex)
    {
        LaneConfig lane = lanes[laneIndex];
        GameObject note = Instantiate(lane.notePrefab, lane.spawnPoint.position, Quaternion.identity, beatScroller.transform);
                
        NoteObject[] noteObjects = note.GetComponentsInChildren<NoteObject>();
        foreach (NoteObject noteObj in noteObjects)
        {
            noteObj.SetupNote(lane.keyToPress, lane.targetPoint.position);
        }

        MinigameManager.instance.NoteSpawned();
    }
}