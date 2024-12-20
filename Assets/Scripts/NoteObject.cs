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
    private Vector3 targetPosition;
    private Vector3 moveDirection;

    public void SetupNote(KeyCode key, Vector3 target)
    {
        keyToPress = key;
        targetPosition = target;
        moveDirection = (targetPosition - transform.position).normalized;
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
        Vector3 toTarget = targetPosition - transform.position;
        float distanceFromCenter = toTarget.magnitude;

        // Determina se está Early ou Late baseado na direção do movimento
        // Se o produto escalar é positivo, significa que ainda não chegou no alvo (Early)
        bool isBeforeTarget = Vector3.Dot(toTarget.normalized, moveDirection) > 0;
        
        // Inverte o sinal da distância se estiver antes do alvo
        if (isBeforeTarget)
        {
            distanceFromCenter = -distanceFromCenter;
        }

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
        Instantiate(effectPrefab, transform.position, Quaternion.identity);
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
            gameObject.SetActive(false);
        }
    }

    public void ResetNote()
    {
        canBePressed = false;
        wasPressed = false;
    }
}