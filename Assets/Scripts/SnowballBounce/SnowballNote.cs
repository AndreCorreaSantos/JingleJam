using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Rigidbody))]
public class SnowballNote : NoteObject
{
    [Header("Snowball Specific")]
    [SerializeField] private TrailRenderer snowballTrail;
    // [SerializeField] private VisualEffect effect;
    [SerializeField] private Color goodColor;
    [SerializeField] private Color badColor;
    
    [Header("Physics Settings")]
    [SerializeField] private float perfectHitForce = 20f;
    [SerializeField] private float badHitForce = 10f;
    [SerializeField] private float perfectUpwardAngle = 45f;
    [SerializeField] private float badHitAngleRange = 30f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    protected override void OnNoteProcessed()
    {
        switch (lastHitAccuracy)
        {
            case HitAccuracy.Perfect:
                HandlePerfectHit();
                break;
            case HitAccuracy.Early:
            case HitAccuracy.Late:
                HandleBadHit(lastHitAccuracy == HitAccuracy.Late);
                break;
            default:
                HandleMiss();
                break;
        }
    }

    private void HandlePerfectHit()
    {
        // if (effect != null)
        // {
        //     // set effect color to good color
        //     effect.SetVector4("Color", goodColor);

        //     effect.Play();
        // }

        rb.isKinematic = false;
        rb.useGravity = true;
        Vector3 hitDirection = Quaternion.Euler(-perfectUpwardAngle, -45, 0) * Vector3.forward;
        rb.AddForce(hitDirection * perfectHitForce, ForceMode.Impulse);

        if (snowballTrail != null)
        {
            snowballTrail.enabled = false;
        }
        
        Destroy(gameObject, 2f);
    }

    private void HandleBadHit(bool isLate)
    {

        rb.isKinematic = false;
        rb.useGravity = true;
        float randomAngle = isLate ? -badHitAngleRange : badHitAngleRange;
        Vector3 hitDirection = Quaternion.Euler(-perfectUpwardAngle + randomAngle, -45, 0) * Vector3.back;
        rb.AddForce(hitDirection * badHitForce, ForceMode.Impulse);
        
        if (snowballTrail != null)
        {
            snowballTrail.enabled = false;
        }
        
        Destroy(gameObject, 2f);
    }

    private void HandleMiss()
    {
        base.SpawnEffect(missEffect);
        
        if (snowballTrail != null)
        {
            snowballTrail.enabled = false;
        }

        SantaBatter santa = Object.FindFirstObjectByType<SantaBatter>();
        if (santa != null)
        {
            santa.GetHit();
        }
        
        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Activator"))
        {
            canBePressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!wasPressed && other.CompareTag("Activator") && gameObject.activeSelf)
        {
            canBePressed = false;
            MinigameManager.instance.NoteMissed();
            HandleMiss();
            gameObject.SetActive(false);
        }
    }
}