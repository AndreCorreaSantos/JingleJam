using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class MenuButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Button References")]
    [SerializeField] private Image buttonBackground;
    [SerializeField] private TextMeshProUGUI buttonText;

    [Header("Animation Settings")]
    [SerializeField] private Color normalTextColor = Color.white;
    [SerializeField] private Color hoverTextColor = Color.red;
    [SerializeField] private Color normalBackgroundColor = new Color(0.2f, 0.2f, 0.2f, 1f);
    [SerializeField] private Color hoverBackgroundColor = new Color(0.3f, 0.3f, 0.3f, 1f);
    [SerializeField] private float hoverScaleMultiplier = 1.1f;
    [SerializeField] private float animationDuration = 0.3f;

    private Vector3 originalScale;
    private Sequence currentAnimation;

    private void Awake()
    {
        // Validate required components
        if (buttonText == null || buttonBackground == null)
        {
            Debug.LogError($"Missing required components on {gameObject.name}", this);
            return;
        }

        // Store the original scale for reference
        originalScale = transform.localScale;

        // Set initial colors
        buttonText.color = normalTextColor;
        buttonBackground.color = normalBackgroundColor;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Check if components are still valid
        if (buttonText == null || buttonBackground == null)
            return;

        // Kill any running animations
        if (currentAnimation != null)
        {
            currentAnimation.Kill();
        }

        // Create a new animation sequence
        currentAnimation = DOTween.Sequence();

        // Add animations to the sequence
        currentAnimation.Join(buttonText.DOColor(hoverTextColor, animationDuration));
        currentAnimation.Join(buttonBackground.DOColor(hoverBackgroundColor, animationDuration));
        currentAnimation.Join(transform.DOScale(originalScale * hoverScaleMultiplier, animationDuration));

        // Add a subtle bounce effect
        currentAnimation.Append(transform.DOPunchScale(Vector3.one * 0.05f, animationDuration * 0.5f, 1, 0.5f));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Check if components are still valid
        if (buttonText == null || buttonBackground == null)
            return;

        // Kill any running animations
        if (currentAnimation != null)
        {
            currentAnimation.Kill();
        }

        // Reset to original state
        currentAnimation = DOTween.Sequence();
        currentAnimation.Join(buttonText.DOColor(normalTextColor, animationDuration));
        currentAnimation.Join(buttonBackground.DOColor(normalBackgroundColor, animationDuration));
        currentAnimation.Join(transform.DOScale(originalScale, animationDuration));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (transform == null)
            return;

        // Add click feedback animation
        transform.DOPunchScale(Vector3.one * -0.2f, animationDuration, 1, 0.5f);
    }

    private void OnDestroy()
    {
        // Kill any running animations when the object is destroyed
        if (currentAnimation != null)
        {
            currentAnimation.Kill();
            currentAnimation = null;
        }
    }
}