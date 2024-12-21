using UnityEngine;

public class SantaBatter : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float swingDuration = 0.5f;
    [SerializeField] private float resetDelay = 0.2f;
    
    private static readonly int SwingTrigger = Animator.StringToHash("Swing");
    private static readonly int HitTrigger = Animator.StringToHash("GetHit");
    private bool isSwinging;
    
    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isSwinging)
        {
            Swing();
        }
    }

    private void Swing()
    {
        isSwinging = true;
        animator.SetTrigger(SwingTrigger);
        Invoke(nameof(ResetSwing), swingDuration + resetDelay);
    }

    private void ResetSwing()
    {
        isSwinging = false;
    }

    public void GetHit()
    {
        animator.ResetTrigger(HitTrigger);
        animator.SetTrigger(HitTrigger);
    }
}