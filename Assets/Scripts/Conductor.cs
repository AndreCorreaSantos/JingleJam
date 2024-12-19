using UnityEngine;
using UnityEngine.Audio;

public class Conductor : MonoBehaviour
{
    public static Conductor instance;
    
    [Header("Song settings")]
    public float songBpm;
    public float firstBeatOffset;
    public float secPerBeat { get; private set; }
    public float songPosition { get; private set; }
    public float songPositionInBeats { get; private set; }
    public float dspSongTime { get; private set; }
    public bool hasStarted { get; private set; }

    [Header("Accuracy Settings")]
    public float perfectThreshold = 0.03f;    // ±30ms
    public float goodThreshold = 0.06f;       // ±60ms
    public float normalThreshold = 0.1f;      // ±100ms

    [Header("Audio Analysis")]
    public float currentBeatVolume { get; private set; }
    private float[] audioData = new float[1024];
    
    public AudioSource musicSource;
    [SerializeField] private BeatScroller beatScroller;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    void Start()
    {        
        if (!musicSource || !beatScroller)
        {
            Debug.LogError("Conductor: Missing required components!");
            enabled = false;
            return;
        }

        secPerBeat = 60f / songBpm;
        hasStarted = false;
    }

    public void StartSong()
    {
        dspSongTime = (float)AudioSettings.dspTime;
        musicSource.Play();
        hasStarted = true;
        beatScroller.hasStarted = true;
    }

    void Update()
    {
        if (!hasStarted) return;

        songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);
        songPositionInBeats = songPosition / secPerBeat;

        // Análise do volume da música
        if (musicSource.isPlaying)
        {
            musicSource.GetSpectrumData(audioData, 0, FFTWindow.BlackmanHarris);
            currentBeatVolume = 0;
            for (int i = 0; i < 8; i++) // Pega as frequências mais baixas (graves)
            {
                currentBeatVolume += audioData[i];
            }
            currentBeatVolume /= 8;
        }
    }

    public HitAccuracy GetNoteAccuracy(float notePosition)
    {
        float distanceFromCenter = Mathf.Abs(notePosition);
        
        if (distanceFromCenter <= perfectThreshold)
            return HitAccuracy.Perfect;
        if (distanceFromCenter <= goodThreshold)
            return HitAccuracy.Good;
        if (distanceFromCenter <= normalThreshold)
            return HitAccuracy.Normal;
            
        return HitAccuracy.Miss;
    }
}
public enum HitAccuracy
{
    Perfect,
    Good,
    Normal,
    Miss
}