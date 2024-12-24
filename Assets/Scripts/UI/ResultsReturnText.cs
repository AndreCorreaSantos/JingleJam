using UnityEngine;
using TMPro;
using DG.Tweening;

public class ResultsReturnText : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI buttonText;

    [Header("Animation Settings")]
    [SerializeField] private float animationDuration = 2f;
    [SerializeField] private float minScale = 0.9f;
    [SerializeField] private float maxScale = 1.1f;
    [SerializeField] private float minAlpha = 0.4f;
    [SerializeField] private float maxAlpha = 1f;

    private Sequence animationSequence;

    private void Start()
    {
        CreateAnimation();
    }

    private void CreateAnimation()
    {
        // Kill any existing sequence to prevent multiple animations
        animationSequence?.Kill();

        // Create a new sequence
        animationSequence = DOTween.Sequence();

        // Start with minimum values
        buttonText.transform.localScale = Vector3.one * minScale;
        buttonText.alpha = minAlpha;

        // Create a complete cycle: min -> max -> min
        animationSequence.Append(
            buttonText.transform.DOScale(maxScale, animationDuration / 2)
                .SetEase(Ease.InOutSine)
        );
        animationSequence.Join(
            buttonText.DOFade(maxAlpha, animationDuration / 2)
                .SetEase(Ease.InOutSine)
        );

        animationSequence.Append(
            buttonText.transform.DOScale(minScale, animationDuration / 2)
                .SetEase(Ease.InOutSine)
        );
        animationSequence.Join(
            buttonText.DOFade(minAlpha, animationDuration / 2)
                .SetEase(Ease.InOutSine)
        );

        // Set the sequence to loop infinitely with yoyo effect
        animationSequence.SetLoops(-1, LoopType.Restart);
    }

    private void OnDestroy()
    {
        // Clean up the sequence when the object is destroyed
        animationSequence?.Kill();
    }
}