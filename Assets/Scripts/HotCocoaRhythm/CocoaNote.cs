using UnityEngine;

public class CocoaNote : NoteObject
{
    [Header("Cocoa Settings")]
    [SerializeField] private Transform chocolateLevel;    
    [SerializeField] private Transform targetIndicator;   
    [SerializeField] private float fillSpeed = 1f;       
    [SerializeField] private float perfectThreshold = 0.05f;
    
    [Header("Height Settings")]
    [SerializeField] private float minPossibleHeight = 0.2f;
    [SerializeField] private float maxPossibleHeight = 0.756f; 
    [SerializeField] private float maxScale = 0.756f;   

    private float currentHeight = 0f;
    private float targetHeight;
    private bool isFillingUp = false;
    private Vector3 originalScale;

    private void Awake()
    {
        targetHeight = Random.Range(minPossibleHeight, maxPossibleHeight);
    }

    private void Start()
    {
        if (chocolateLevel != null)
        {
            originalScale = chocolateLevel.localScale;
            SetChocolateHeight(0f);
        }
        
        if (targetIndicator != null)
        {
            Vector3 indicatorPos = targetIndicator.localPosition;
            indicatorPos.y = targetHeight;
            targetIndicator.localPosition = indicatorPos;
        }
    }

    void Update()
    {
        if (!canBePressed || wasPressed)
        {
            return;
        }
        if (canBePressed)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                if (!isFillingUp)
                {
                    isFillingUp = true;
                }
                currentHeight += fillSpeed * Time.deltaTime;
                currentHeight = Mathf.Min(currentHeight, maxScale); 
                SetChocolateHeight(currentHeight);
            }
            
            if (isFillingUp && Input.GetKeyUp(KeyCode.Space))
            {
                wasPressed = true;
                HandleNoteCompletion();
            }
        }
    }

    private void SetChocolateHeight(float height)
    {
        if (chocolateLevel == null) return;
        
        Vector3 newScale = originalScale;
        newScale.y = height / maxScale;
        chocolateLevel.localScale = newScale;
    }
    // Sobrescrevemos o HandleNotePress da classe base para fazer nada
    protected override void HandleNotePress()
    {
        // Não faz nada - ignora completamente o comportamento da classe base
    }
    private void HandleNoteCompletion()
    {
        float heightDifference = Mathf.Abs(currentHeight - targetHeight);
        
        if (heightDifference <= perfectThreshold)
        {
            MinigameManager.instance.PerfectHit();
            lastHitAccuracy = HitAccuracy.Perfect;
            SpawnEffect(perfectEffect);
        }
        else if (currentHeight < targetHeight)
        {
            MinigameManager.instance.EarlyHit();
            lastHitAccuracy = HitAccuracy.Early;
            SpawnEffect(earlyEffect);
        }
        else
        {
            MinigameManager.instance.LateHit();
            lastHitAccuracy = HitAccuracy.Late;
            SpawnEffect(lateEffect);
        }

        HandleNotePress();
    }

    protected override void SpawnEffect(GameObject effectPrefab)
    {
        Instantiate(effectPrefab, transform.position + Vector3.up * 1f, Quaternion.LookRotation(Camera.main.transform.forward));
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
            if (isFillingUp)
            {
                HandleNoteCompletion();
            }
            else
            {
                MinigameManager.instance.NoteMissed();
                lastHitAccuracy = HitAccuracy.Miss;
                SpawnEffect(missEffect);
                OnNoteProcessed();
            }
        }
    }

    public void ResetCocoa()
    {
        currentHeight = 0f;
        isFillingUp = false;
        SetChocolateHeight(0f);
        ResetNote();
    }
}