using UnityEngine;

public class NoteObject : MonoBehaviour
{
    [Header("Visual Effects")]
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private GameObject goodEffect;
    [SerializeField] private GameObject perfectEffect;
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
        distanceFromCenter = Mathf.Abs(transform.position.y);
        HitAccuracy accuracy = Conductor.instance.GetNoteAccuracy(distanceFromCenter);
        
        GameObject effectToSpawn = null;
        
        switch (accuracy)
        {
            case HitAccuracy.Perfect:
                MinigameManager.instance.PerfectHit();
                effectToSpawn = perfectEffect;
                break;
            case HitAccuracy.Good:
                MinigameManager.instance.GoodHit();
                effectToSpawn = goodEffect;
                break;
            case HitAccuracy.Normal:
                MinigameManager.instance.NormalHit();
                effectToSpawn = hitEffect;
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