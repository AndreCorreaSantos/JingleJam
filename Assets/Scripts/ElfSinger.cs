using UnityEngine;

public class ElfSinger : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem noteParticles;
    
    private static readonly int SingTrigger = Animator.StringToHash("Sing");
    private static readonly int IntensityParam = Animator.StringToHash("Intensity");

    private void Start()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    public void Sing(float intensity)
    {
        animator.SetFloat(IntensityParam, intensity);
        animator.SetTrigger(SingTrigger);

        if (noteParticles != null)
        {
            if (intensity > 0)
            {
                noteParticles.Play();
                var emission = noteParticles.emission;
                emission.rateOverTime = intensity * 10f; // Mais partículas para intensidade maior
            }
            else
            {
                noteParticles.Stop();
            }
        }
    }
}