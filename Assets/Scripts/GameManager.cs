using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{

    public AudioSource audioSource;
    [SerializeField] private bool startPlaying;
    [SerializeField] private BeatScroller beatScroller;
    public int  currentScore;
    public int scorePerNote = 100;
    public int scorePerGoodNote = 125;
    public int scorePerPerfectNote = 150;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text multiText;
    [SerializeField] private int currentMultiplier;
    [SerializeField] private int multiplierTracker;
    [SerializeField] private int[] multiplierThresholds;

    [SerializeField] private int totalNotes;
    [SerializeField] private int normalHits, goodHits, perfectHits, missedHits;

    [SerializeField] private GameObject resultsScreen;
    [SerializeField] private Text percentHitText, normalsText, goodsText, perfectsText, missesText, rankText, finalScoreText;
    public static GameManager instance;

    void Start()
    {
        instance = this;
        scoreText.text = "Score: 0";
        currentMultiplier = 1;
        totalNotes = FindObjectsOfType<NoteObject>().Length;
    }

    void Update()
    {
        if (!startPlaying)
        {
            if (Input.anyKeyDown)
            {
                startPlaying = true;
                beatScroller.hasStarted = true;
                audioSource.Play();
            }
        } else
        {
            if (!audioSource.isPlaying && !resultsScreen.activeInHierarchy)
            {
                resultsScreen.SetActive(true);
                normalsText.text = "" + normalHits;
                goodsText.text = "" + goodHits;
                perfectsText.text = "" + perfectHits;
                missesText.text = "" + missedHits;
                int totalHit = normalHits + goodHits + perfectHits;
                float percentHit = (float)totalHit / (float)totalNotes * 100f;
                percentHitText.text = percentHit.ToString("F1") + "%";

                string rankVal = "F";
                if (percentHit > 40)
                {
                    rankVal = "D";
                }
                if (percentHit > 55)
                {
                    rankVal = "C";
                }
                if (percentHit > 70)
                {
                    rankVal = "B";
                }
                if (percentHit > 85)
                {
                    rankVal = "A";
                }
                if (percentHit > 95)
                {
                    rankVal = "S";
                }
                rankText.text = rankVal;

                finalScoreText.text = "" + currentScore;
            }
        }
    }

    public void NoteHit()
    {
        //Debug.Log("Hit On Time");
        if (currentMultiplier - 1 < multiplierThresholds.Length)
        {
            multiplierTracker++;
            if (multiplierTracker >= multiplierThresholds[currentMultiplier - 1])
            {
                multiplierTracker = 0;
                currentMultiplier++;
            }
        }
        //currentScore += scorePerNote * currentMultiplier;
        multiText.text = "Multiplier: x" + currentMultiplier;
        scoreText.text = "Score: " + currentScore;
    }

    public void NoteMissed()
    {
        Debug.Log("Missed Note");
        currentMultiplier = 1;
        multiplierTracker = 0;
        multiText.text = "Multiplier: x" + currentMultiplier;
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