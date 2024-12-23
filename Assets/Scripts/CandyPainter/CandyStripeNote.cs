using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CandyStripeNote : NoteObject
{
    [Header("Paint Materials")]
    [SerializeField] private Material perfectPaintMaterial;
    [SerializeField] private Material badPaintMaterial;
    [SerializeField] private Material missPaintMaterial;
    [SerializeField] private MeshRenderer noteRenderer;
    
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
    }

    protected override void OnNoteProcessed()
    {
        Material materialToUse = null;
        
        switch (lastHitAccuracy)
        {
            case HitAccuracy.Perfect:
                materialToUse = perfectPaintMaterial;
                break;
            case HitAccuracy.Early:
            case HitAccuracy.Late:
                materialToUse = badPaintMaterial;
                break;
            default:
                materialToUse = missPaintMaterial;
                break;
        }

        if (noteRenderer != null && materialToUse != null)
        {
            noteRenderer.material = materialToUse;
        }
        
    }

    protected override void SpawnEffect(GameObject effectPrefab)
    {
        Instantiate(effectPrefab, transform.position + Vector3.left * 3f, Quaternion.LookRotation(Camera.main.transform.forward));
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
            SpawnEffect(missEffect);
            OnNoteProcessed();
        }
    }
}