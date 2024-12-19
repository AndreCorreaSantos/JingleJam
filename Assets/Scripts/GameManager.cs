using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game Settings")]
    [SerializeField] private bool startPlaying;
    [SerializeField] private int[] multiplierThresholds;
    [SerializeField] private float delayAfterSongEnd = 2f; 

    [Header("Scoring")]
    public int scorePerNote = 100;
    public int scorePerGoodNote = 125;
    public int scorePerPerfectNote = 150;
    
    [Header("UI References")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Text multiText;
    [SerializeField] private GameObject resultsScreen;
    [SerializeField] private Text percentHitText;
    [SerializeField] private Text normalsText;
    [SerializeField] private Text goodsText;
    [SerializeField] private Text perfectsText;
    [SerializeField] private Text missesText;
    [SerializeField] private Text rankText;
    [SerializeField] private Text finalScoreText;

    private int currentScore;
    private int currentMultiplier = 1;
    private int multiplierTracker;
    private int totalNotes;
    private int normalHits;
    private int goodHits;
    private int perfectHits;
    private int missedHits;
    private float songEndTime;
    private bool isSongEnded;

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
        InitializeGame();
    }

    private void InitializeGame()
    {
        scoreText.text = "Score: 0";
        currentMultiplier = 1;
        multiplierTracker = 0;
        totalNotes = 0;
        isSongEnded = false;
        UpdateMultiplierText();
    }

    void Update()
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

    private void CheckGameStart()
    {
        if (Input.anyKeyDown)
        {
            startPlaying = true;
            Conductor.instance.StartSong();
        }
    }

    private void CheckGameEnd()
    {
        if (!isSongEnded && !Conductor.instance.musicSource.isPlaying)
        {
            isSongEnded = true;
            songEndTime = Time.time;
            
            RhythmSpawner spawner = FindFirstObjectByType<RhythmSpawner>();
            if (spawner != null)
            {
                spawner.enabled = false;
            }
        }

        if (isSongEnded && Time.time >= songEndTime + delayAfterSongEnd && !resultsScreen.activeInHierarchy)
        {
            ShowResults();
        }
    }

    private void ShowResults()
    {
        resultsScreen.SetActive(true);
        UpdateResultsUI();
        CalculateAndShowRank();
    }

    private void UpdateResultsUI()
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

    private void CalculateAndShowRank()
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

    public void NoteSpawned()
    {
        totalNotes++;
    }

    public void NoteHit()
    {
        UpdateMultiplier();
        UpdateUI();
    }

    private void UpdateMultiplier()
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

    private void UpdateUI()
    {
        scoreText.text = "Score: " + currentScore;
        UpdateMultiplierText();
    }

    private void UpdateMultiplierText()
    {
        multiText.text = "Multiplier: x" + currentMultiplier;
    }

    public void NoteMissed()
    {
        currentMultiplier = 1;
        multiplierTracker = 0;
        UpdateMultiplierText();
        missedHits++;
    }

    public void NormalHit()
    {
        currentScore += scorePerNote * currentMultiplier;
        NoteHit();
        normalHits++;
    }

    public void GoodHit()
    {
        currentScore += scorePerGoodNote * currentMultiplier;
        NoteHit();
        goodHits++;
    }

    public void PerfectHit()
    {
        currentScore += scorePerPerfectNote * currentMultiplier;
        NoteHit();
        perfectHits++;
    }
}