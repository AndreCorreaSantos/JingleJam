using UnityEngine;
using System.Collections;
using UnityEngine.VFX;

public class NoteObject : MonoBehaviour
{
    [Header("Visual Effects")]
    [SerializeField] protected GameObject earlyEffect;
    [SerializeField] protected GameObject perfectEffect;
    [SerializeField] protected GameObject lateEffect;
    [SerializeField] protected GameObject missEffect;

    protected KeyCode keyToPress;
    protected bool canBePressed;
    protected bool wasPressed;
    protected Vector3 targetPosition;
    protected Vector3 moveDirection;
    protected HitAccuracy lastHitAccuracy;

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

    protected virtual void HandleNotePress()
    {
        Vector3 toTarget = targetPosition - transform.position;
        float distanceFromCenter = toTarget.magnitude;

        bool isBeforeTarget = Vector3.Dot(toTarget.normalized, moveDirection) > 0;

        if (isBeforeTarget)
        {
            distanceFromCenter = -distanceFromCenter;
        }

        lastHitAccuracy = Conductor.instance.GetNoteAccuracy(distanceFromCenter);
        GameObject effectToSpawn = null;

        switch (lastHitAccuracy)
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

        OnNoteProcessed();
    }

    protected virtual void OnNoteProcessed()
    {
        gameObject.SetActive(false);
    }

    protected virtual void SpawnEffect(GameObject effectPrefab)
    {   

        GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.LookRotation(Camera.main.transform.forward));
        VisualEffect vfx = effect.GetComponent<VisualEffect>();
        if (vfx != null)
        {
            vfx.playRate *= 2f;
        }
    }

    private IEnumerator playEffect(GameObject effectPrefab)
    {
        GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.LookRotation(Camera.main.transform.forward));
        yield return new WaitForSeconds(1f);
        Destroy(effect);
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
            if (MinigameManager.instance != null)
            {
                MinigameManager.instance.NoteMissed();
            }
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