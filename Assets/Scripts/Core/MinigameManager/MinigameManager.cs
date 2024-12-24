using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

[System.Serializable]
public class MinigameResults
{
    public string minigameName;
    public int score;
    public float percentageHit;
    public int earlyHits;
    public int perfectHits;
    public int lateHits;
    public int missedHits;
    public string rank;
    public float finalMultiplier;
    public float maxMultiplier;
}

public abstract class MinigameManager : MonoBehaviour
{
    public static MinigameManager instance;

    [Header("Minigame Base Settings")]
    protected MinigameData minigameData;
    protected MinigameState currentState = MinigameState.Ready;
    protected float baseScore = 0;

    protected enum MinigameState
    {
        Ready,      // Initial state
        Playing,    // Game is active
        Completed,  // Game is finished
        Failed      // Player failed the game
    }

    // Events
    public event Action OnMinigameCompleted;

    [Header("Rhythm Game Settings")]
    private float delayAfterSongEnd = .5f;

    [Header("Scoring")]
    public int scorePerEarlyLateNote = 50;
    public int scorePerPerfectNote = 100;

    [Header("Camera")]
    [SerializeField] protected CameraShake cameraShake;

    [Header("TutorialPanel")]
    [SerializeField] protected Animator tutorialAnimator;
    
    [Header("Start Settings")]
    [SerializeField] private float startDelay = 5f; 
    private float startTimer;
    private bool startPlaying;

    [Header("UI References")]
    [SerializeField] protected TextMeshProUGUI scoreText;
    [SerializeField] protected TextPunchAnimation scorePunchScript;
    [SerializeField] protected Text multiText;
    [SerializeField] protected GameObject resultsScreen;
    [SerializeField] protected Text percentHitText;
    [SerializeField] protected Text earlysText;
    [SerializeField] protected Text perfectsText;
    [SerializeField] protected Text latesText;
    [SerializeField] protected Text missesText;
    [SerializeField] protected Text rankText;
    [SerializeField] protected Text finalScoreText;

    // Rhythm game specific variables
    protected int currentScore;
    protected float currentMultiplier;
    protected float maxMultiplier;
    protected int totalNotes;
    protected int earlyHits;
    protected int perfectHits;
    protected int lateHits;
    protected int missedHits;
    protected float songEndTime;
    protected bool isSongEnded;

    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    protected virtual void Start()
    {
        MatchManager.Instance.OnMultiplierChanged += HandleMultiplierChanged;
        MatchManager.Instance.SetMinigameManager(this);
        minigameData = MatchManager.Instance.GetCurrentMinigameData();
        PrepareMinigame();
    }



    protected virtual void PrepareMinigame()
    {
        currentState = MinigameState.Ready;
        InitializeGame();
        StartMinigame();
    }

    protected virtual void StartMinigame()
    {
        tutorialAnimator.SetTrigger("Play");
        currentState = MinigameState.Playing;
    }

    public virtual void CompleteMinigame()
    {
        if (currentState != MinigameState.Playing) return;

        currentState = MinigameState.Completed;

        MinigameResults results = new MinigameResults
        {
            minigameName = minigameData.minigameName,
            score = currentScore,
            percentageHit = totalNotes > 0 ? (float)(earlyHits + perfectHits + lateHits) / totalNotes * 100f : 0f,
            earlyHits = earlyHits,
            perfectHits = perfectHits,
            lateHits = lateHits,
            missedHits = missedHits,
            rank = CalculateRank(),
            finalMultiplier = currentMultiplier,
            maxMultiplier = maxMultiplier
        };

        MatchManager.Instance.StoreMinigameResults(results);

        OnMinigameCompleted?.Invoke();

        // ShowResults();

        // Delay the loading of next minigame
        Invoke("LoadNextMinigame", delayAfterSongEnd);
    }

    private string CalculateRank()
    {
        int totalHit = earlyHits + perfectHits + lateHits;
        float percentHit = totalNotes > 0 ? (float)totalHit / totalNotes * 100f : 0f;
        float perfectPercent = totalNotes > 0 ? (float)perfectHits / totalNotes * 100f : 0f;

        if (percentHit > 95 && perfectPercent > 80) return "S";
        if (percentHit > 90 && perfectPercent > 60) return "A";
        if (percentHit > 80) return "B";
        if (percentHit > 70) return "C";
        if (percentHit > 60) return "D";
        return "F";
    }

    protected virtual void LoadNextMinigame()
    {
        MatchManager.Instance.NextMinigame();
    }

    protected virtual void InitializeGame()
    {
        scoreText.text = "Score: 0";
        currentMultiplier = MatchManager.Instance.GetCurrentMultiplier();
        currentScore = 0;
        totalNotes = 0;
        earlyHits = 0;
        perfectHits = 0;
        lateHits = 0;
        missedHits = 0;
        isSongEnded = false;
        startTimer = startDelay;
        startPlaying = false;
        UpdateMultiplierText();
    }

    protected virtual void Update()
    {
        if (!startPlaying)
        {
            startTimer -= Time.deltaTime;
            if (startTimer <= 0)
            {
                startPlaying = true;
                if (Conductor.instance != null)
                {
                    Conductor.instance.StartSong();
                }
            }
        }
        else
        {
            CheckGameEnd();
        }
    }

    protected virtual void CheckGameEnd()
    {
        if (!isSongEnded && Conductor.instance != null && !Conductor.instance.musicSource.isPlaying)
        {
            isSongEnded = true;
            songEndTime = Time.time;

            RhythmSpawner spawner = FindFirstObjectByType<RhythmSpawner>();
            if (spawner != null)
            {
                spawner.enabled = false;
            }
        }

        if (isSongEnded && Time.time >= songEndTime + delayAfterSongEnd)
        {
            CompleteMinigame();
        }
    }

    private void HandleMultiplierChanged(float newMultiplier)
    {
        maxMultiplier = Mathf.Max(maxMultiplier, newMultiplier);
        currentMultiplier = newMultiplier;
        UpdateMultiplierText();
    }

    protected virtual void UpdateResultsUI()
    {
        earlysText.text = earlyHits.ToString();
        perfectsText.text = perfectHits.ToString();
        latesText.text = lateHits.ToString();
        missesText.text = missedHits.ToString();

        int totalHit = earlyHits + perfectHits + lateHits;
        float percentHit = totalNotes > 0 ? (float)totalHit / totalNotes * 100f : 0f;
        percentHitText.text = percentHit.ToString("F1") + "%";
        finalScoreText.text = currentScore.ToString();
    }

    // Public methods for note tracking and scoring
    public virtual void NoteSpawned()
    {
        totalNotes++;
    }

    public virtual void NoteHit()
    {
        UpdateUI();
    }

    protected virtual void UpdateUI()
    {
        scoreText.text = "Score: " + currentScore;
        UpdateMultiplierText();
    }

    protected virtual void UpdateMultiplierText()
    {
        multiText.text = $"x{currentMultiplier:F1}";
    }

    public virtual void NoteMissed()
    {
        AudioManager.Instance.PlaySound("NoteMissed");
        MatchManager.Instance.SetCurrentMultiplier(1f);
        missedHits++;
        UpdateUI();
    }

    public virtual void EarlyHit()
    {
        AudioManager.Instance.PlaySound("MediumHit");
        currentScore += (int)(scorePerEarlyLateNote * currentMultiplier);
        earlyHits++;
        NoteHit();
    }

    public virtual void PerfectHit()
    {
        cameraShake.StartShake(0.25f, 0.05f);
        AudioManager.Instance.PlaySound("PerfectHit");
        scorePunchScript.PlayPunchAnimation();
        currentScore += (int)(scorePerPerfectNote * currentMultiplier);
        float newMultiplier = Mathf.Min(currentMultiplier + 0.1f, 4f);
        MatchManager.Instance.SetCurrentMultiplier(newMultiplier);
        perfectHits++;
        NoteHit();
    }

    public virtual void LateHit()
    {
        AudioManager.Instance.PlaySound("MediumHit");
        currentScore += (int)(scorePerEarlyLateNote * currentMultiplier);
        lateHits++;
        NoteHit();
    }

    private void OnDestroy()
    {
        MatchManager.Instance.OnMultiplierChanged -= HandleMultiplierChanged;
        instance = null;
    }
}