using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{

    public AudioSource audioSource;
    [SerializeField] private bool startPlaying;
    [SerializeField] private BeatScroller beatScroller;
    public int  currentScore;
    public int scorePerNote = 100;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text multiText;
    [SerializeField] private int currentMultiplier;
    [SerializeField] private int multiplierTracker;
    [SerializeField] private int[] multiplierThresholds;
    public static GameManager instance;

    void Start()
    {
        instance = this;
        scoreText.text = "Score: 0";
        currentMultiplier = 1;
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
        }
    }

    public void NoteHit()
    {
        Debug.Log("Hit On Time");
        if (currentMultiplier - 1 < multiplierThresholds.Length)
        {
            multiplierTracker++;
            if (multiplierTracker >= multiplierThresholds[currentMultiplier - 1])
            {
                multiplierTracker = 0;
                currentMultiplier++;
            }
        }
        currentScore += scorePerNote * currentMultiplier;
        multiText.text = "Multiplier: x" + currentMultiplier;
        scoreText.text = "Score: " + currentScore;
    }

    public void NoteMissed()
    {
        Debug.Log("Missed Note");
        currentMultiplier = 1;
        multiplierTracker = 0;
        multiText.text = "Multiplier: x" + currentMultiplier;
    }

}