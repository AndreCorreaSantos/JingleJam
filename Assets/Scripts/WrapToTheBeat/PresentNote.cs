using UnityEngine;

public class PresentNote : NoteObject
{
    [Header("Present Visuals")]
    [SerializeField] private GameObject unwrappedBox;
    [SerializeField] private GameObject perfectWrappedPresent;
    [SerializeField] private GameObject messyWrappedPresent;
    
    protected override void OnNoteProcessed()
    {
        if (lastHitAccuracy != HitAccuracy.Miss && unwrappedBox != null)
        {
            unwrappedBox.SetActive(false);
        }
        switch (lastHitAccuracy)
        {
            case HitAccuracy.Perfect:
                if (perfectWrappedPresent != null)
                {
                    perfectWrappedPresent.SetActive(true);
                }
                break;
                
            case HitAccuracy.Early:
            case HitAccuracy.Late:
                if (messyWrappedPresent != null)
                {
                    messyWrappedPresent.SetActive(true);
                }
                break;
                
            default:
                if (unwrappedBox != null)
                {
                    unwrappedBox.SetActive(true);
                }
                break;
        }
    }

    public void ResetPresent()
    {
        if (unwrappedBox != null) unwrappedBox.SetActive(true);
        if (perfectWrappedPresent != null) perfectWrappedPresent.SetActive(false);
        if (messyWrappedPresent != null) messyWrappedPresent.SetActive(false);
        
        ResetNote();
    }
    
    protected override void SpawnEffect(GameObject effectPrefab)
    {
        // Ajusta a posição do efeito para aparecer um pouco acima do presente
        Vector3 effectPosition = transform.position + Vector3.up * 2f;
        Instantiate(effectPrefab, effectPosition, Quaternion.LookRotation(Camera.main.transform.forward));
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
            lastHitAccuracy = HitAccuracy.Miss;
            MinigameManager.instance.NoteMissed();
            SpawnEffect(missEffect);
            OnNoteProcessed();
        }
    }
}