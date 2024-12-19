using UnityEngine;
using UnityEngine.UI;
using System;

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
    [SerializeField] private bool startPlaying;
    [SerializeField] private int[] multiplierThresholds;
    [SerializeField] private float delayAfterSongEnd = 2f;

    [Header("Scoring")]
    public int scorePerNote = 100;
    public int scorePerGoodNote = 125;
    public int scorePerPerfectNote = 150;
    
    [Header("UI References")]
    [SerializeField] protected Text scoreText;
    [SerializeField] protected Text multiText;
    [SerializeField] protected GameObject resultsScreen;
    [SerializeField] protected Text percentHitText;
    [SerializeField] protected Text normalsText;
    [SerializeField] protected Text goodsText;
    [SerializeField] protected Text perfectsText;
    [SerializeField] protected Text missesText;
    [SerializeField] protected Text rankText;
    [SerializeField] protected Text finalScoreText;

    // Rhythm game specific variables
    protected int currentScore;
    protected int currentMultiplier = 1;
    protected int multiplierTracker;
    protected int totalNotes;
    protected int normalHits;
    protected int goodHits;
    protected int perfectHits;
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
        minigameData = GameManager.Instance.GetCurrentMinigameData();
        if (minigameData == null)
        {
            Debug.LogError("No minigame data found!");
            return;
        }
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
        currentState = MinigameState.Playing;
    }

    protected virtual void CompleteMinigame()
    {
        if (currentState != MinigameState.Playing) return;

        currentState = MinigameState.Completed;
        OnMinigameCompleted?.Invoke();
        
        ShowResults();
        
        // Delay the loading of next minigame
        Invoke("LoadNextMinigame", delayAfterSongEnd);
    }

    protected virtual void LoadNextMinigame()
    {
        GameManager.Instance.LoadNextMinigame();
    }

    protected virtual void InitializeGame()
    {
        scoreText.text = "Score: 0";
        currentMultiplier = 1;
        multiplierTracker = 0;
        totalNotes = 0;
        isSongEnded = false;
        UpdateMultiplierText();
    }

    protected virtual void Update()
    {
        if (!startPlaying)
        {
            CheckGameStart();
        }
        else
        {
            CheckGameEnd();
        }
    }

    protected virtual void CheckGameStart()
    {
        if (Input.anyKeyDown)
        {
            startPlaying = true;
            if (Conductor.instance != null)
            {
                Conductor.instance.StartSong();
            }
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
    protected virtual void ShowResults()
    {
        resultsScreen.SetActive(true);
        UpdateResultsUI();
        CalculateAndShowRank();
    }

    protected virtual void UpdateResultsUI()
    {
        normalsText.text = normalHits.ToString();
        goodsText.text = goodHits.ToString();
        perfectsText.text = perfectHits.ToString();
        missesText.text = missedHits.ToString();
        
        int totalHit = normalHits + goodHits + perfectHits;
        float percentHit = totalNotes > 0 ? (float)totalHit / totalNotes * 100f : 0f;
        percentHitText.text = percentHit.ToString("F1") + "%";
        finalScoreText.text = currentScore.ToString();
    }

    protected virtual void CalculateAndShowRank()
    {
        int totalHit = normalHits + goodHits + perfectHits;
        float percentHit = totalNotes > 0 ? (float)totalHit / totalNotes * 100f : 0f;
        
        string rankVal = "F";
        if (percentHit > 95) rankVal = "S";
        else if (percentHit > 85) rankVal = "A";
        else if (percentHit > 70) rankVal = "B";
        else if (percentHit > 55) rankVal = "C";
        else if (percentHit > 40) rankVal = "D";
        
        rankText.text = rankVal;
    }

    // Public methods for note tracking and scoring
    public virtual void NoteSpawned()
    {
        totalNotes++;
    }

    public virtual void NoteHit()
    {
        UpdateMultiplier();
        UpdateUI();
    }

    protected virtual void UpdateMultiplier()
    {
        if (currentMultiplier - 1 < multiplierThresholds.Length)
        {
            multiplierTracker++;
            if (multiplierTracker >= multiplierThresholds[currentMultiplier - 1])
            {
                multiplierTracker = 0;
                currentMultiplier++;
            }
        }
    }

    protected virtual void UpdateUI()
    {
        scoreText.text = "Score: " + currentScore;
        UpdateMultiplierText();
    }

    protected virtual void UpdateMultiplierText()
    {
        multiText.text = "Multiplier: x" + currentMultiplier;
    }

    public virtual void NoteMissed()
    {
        currentMultiplier = 1;
        multiplierTracker = 0;
        UpdateMultiplierText();
        missedHits++;
    }

    public virtual void NormalHit()
    {
        currentScore += scorePerNote * currentMultiplier;
        NoteHit();
        normalHits++;
    }

    public virtual void GoodHit()
    {
        currentScore += scorePerGoodNote * currentMultiplier;
        NoteHit();
        goodHits++;
    }

    public virtual void PerfectHit()
    {
        currentScore += scorePerPerfectNote * currentMultiplier;
        NoteHit();
        perfectHits++;
    }
}