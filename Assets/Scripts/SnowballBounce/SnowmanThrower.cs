using UnityEngine;

public class SnowmanThrower : MonoBehaviour
{
    [Header("Pop Animation")]
    [SerializeField] private float growDuration = 0.2f;
    [SerializeField] private float shrinkDuration = 0.3f;
    [SerializeField] private float maxScaleMultiplier = 1.2f;
    [SerializeField] private AnimationCurve growCurve = AnimationCurve.EaseInOut(0, 1, 1, 1);
    
    [Header("Dance Settings")]
    [SerializeField] private float danceSpeed = 2f;
    [SerializeField] private float danceMagnitude = 0.3f;
    [SerializeField] private float bobSpeed = 1.5f;
    [SerializeField] private float bobMagnitude = 0.1f;
    
    private Vector3 originalScale;
    private Vector3 targetScale;
    private Vector3 originalPosition;
    private float animationTime;
    private float danceTime;
    private bool isAnimating;
    private bool isGrowing;

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale * maxScaleMultiplier;
        originalPosition = transform.position;
    }

    void Update()
    {
        HandleThrowAnimation();
        HandleDanceAnimation();
    }

    private void HandleThrowAnimation()
    {
        if (!isAnimating) return;

        animationTime += Time.deltaTime / (isGrowing ? growDuration : shrinkDuration);

        if (animationTime >= 1f)
        {
            animationTime = 0f;
            
            if (isGrowing)
            {
                isGrowing = false;
            }
            else
            {
                isAnimating = false;
                transform.localScale = originalScale;
            }
        }
        else
        {
            float curveValue = growCurve.Evaluate(animationTime);
            transform.localScale = Vector3.Lerp(
                isGrowing ? originalScale : targetScale,
                isGrowing ? targetScale : originalScale,
                curveValue
            );
        }
    }

    private void HandleDanceAnimation()
    {
        danceTime += Time.deltaTime;

        float sideMovement = Mathf.Sin(danceTime * danceSpeed) * danceMagnitude;
        float verticalMovement = Mathf.Sin(danceTime * bobSpeed) * bobMagnitude;
        
        Vector3 newPosition = originalPosition;
        newPosition.x += sideMovement;
        newPosition.y += verticalMovement;
        
        if (isAnimating && isGrowing)
        {
            float throwBounce = growCurve.Evaluate(animationTime) * 0.2f;
            newPosition.y += throwBounce;
        }
        
        transform.position = newPosition;
        
        float tiltAngle = -sideMovement * 15f; 
        transform.rotation = Quaternion.Euler(0, 180, tiltAngle);
    }

    public void ThrowAnimation()
    {
        isAnimating = true;
        isGrowing = true;
        animationTime = 0f;
    }
}