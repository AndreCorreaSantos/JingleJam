using UnityEngine;

public class GameManager : MonoBehaviour
{

    public AudioSource audioSource;
    [SerializeField] private bool startPlaying;
    [SerializeField] private BeatScroller beatScroller;
    public static GameManager instance;

    void Start()
    {
        instance = this;
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
    }

    public void NoteMissed()
    {
        Debug.Log("Missed Note");
    }

}