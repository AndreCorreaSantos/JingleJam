using UnityEngine;

public class NoteObject : MonoBehaviour
{
    [Header("Visual Effects")]
    [SerializeField] private GameObject earlyEffect;
    [SerializeField] private GameObject perfectEffect;
    [SerializeField] private GameObject lateEffect;
    [SerializeField] private GameObject missEffect;

    private KeyCode keyToPress;
    private bool canBePressed;
    private bool wasPressed;
    private float distanceFromCenter;

    public void SetupNote(KeyCode key)
    {
        keyToPress = key;
        ResetNote();
    }

    void Update()
    {
        if (!canBePressed || wasPressed) return;

        if (Input.GetKeyDown(keyToPress))
        {
            wasPressed = true;
            HandleNotePress();
        }
    }

    private void HandleNotePress()
    {
        // We now use the actual position value, not absolute, to determine Early/Late
        distanceFromCenter = transform.position.y;
        HitAccuracy accuracy = Conductor.instance.GetNoteAccuracy(distanceFromCenter);
        
        GameObject effectToSpawn = null;
        
        switch (accuracy)
        {
            case HitAccuracy.Perfect:
                MinigameManager.instance.PerfectHit();
                effectToSpawn = perfectEffect;
                break;
            case HitAccuracy.Early:
                MinigameManager.instance.EarlyHit();
                effectToSpawn = earlyEffect;
                break;
            case HitAccuracy.Late:
                MinigameManager.instance.LateHit();
                effectToSpawn = lateEffect;
                break;
            default:
                MinigameManager.instance.NoteMissed();
                effectToSpawn = missEffect;
                break;
        }

        if (effectToSpawn)
        {
            SpawnEffect(effectToSpawn);
        }

        gameObject.SetActive(false);
    }

    private void SpawnEffect(GameObject effectPrefab)
    {
        Vector3 spawnPosition = transform.position;
        Quaternion spawnRotation = effectPrefab.transform.rotation;
        Instantiate(effectPrefab, spawnPosition, spawnRotation);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Activator"))
        {
            canBePressed = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!wasPressed && other.CompareTag("Activator") && gameObject.activeSelf)
        {
            canBePressed = false;
            MinigameManager.instance.NoteMissed();
            SpawnEffect(missEffect);
        }
    }

    public void ResetNote()
    {
        canBePressed = false;
        wasPressed = false;
    }
}