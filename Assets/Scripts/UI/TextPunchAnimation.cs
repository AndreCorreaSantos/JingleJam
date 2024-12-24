using UnityEngine;
using TMPro;
using DG.Tweening;

public class TextPunchAnimation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI targetText;

    [Header("Punch Settings")]
    [SerializeField] private float punchStrength = 0.5f;
    [SerializeField] private float punchDuration = 0.3f;
    [SerializeField] private int vibrationCount = 5;
    [SerializeField] private float elasticity = 0.5f;

    private Vector3 originalScale;

    private void Start()
    {
        // Auto-get component if not assigned
        if (targetText == null)
        {
            targetText = GetComponent<TextMeshProUGUI>();
        }
        originalScale = targetText.transform.localScale;
    }

    public void PlayPunchAnimation()
    {
        // Create punch sequence
        Sequence punchSequence = DOTween.Sequence();

        // Add punch scale animation
        punchSequence.Append(
            targetText.transform.DOPunchScale(
                Vector3.one * punchStrength,  // Punch by this amount
                punchDuration,                // Duration of punch
                vibrationCount,               // Number of vibrations
                elasticity                    // Elasticity of punch
            )
        );

        // Ensure we return to original scale
        punchSequence.OnComplete(() =>
        {
            targetText.transform.localScale = originalScale;
        });
    }



    private void OnDestroy()
    {
        // Kill any ongoing tweens on this text
        targetText.transform.DOKill();
    }
}