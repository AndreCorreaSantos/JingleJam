using UnityEngine;

public class ElfWrapper : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float wrapDuration = 0.5f;
    [SerializeField] private float resetDelay = 0.2f;
    
    private static readonly int WrapTrigger = Animator.StringToHash("Wrap");
    private bool isWrapping;
    
    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isWrapping)
        {
            Wrap();
        }
    }

    private void Wrap()
    {
        isWrapping = true;
        animator.SetTrigger(WrapTrigger);
        Invoke(nameof(ResetWrap), wrapDuration + resetDelay);
    }

    private void ResetWrap()
    {
        isWrapping = false;
    }
}