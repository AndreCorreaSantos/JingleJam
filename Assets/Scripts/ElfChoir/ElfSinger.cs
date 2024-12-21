using UnityEngine;

public class ElfSinger : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem noteParticles;
    
    [Header("Maestro Response")]
    [SerializeField] private float tiltAmount = 20f; // Graus de inclinação
    [SerializeField] private float tiltSpeed = 5f; // Velocidade da inclinação
    [SerializeField] private float returnSpeed = 3f; // Velocidade de retorno
    
    private static readonly int SingTrigger = Animator.StringToHash("Sing");
    private static readonly int IntensityParam = Animator.StringToHash("Intensity");
    
    private Quaternion targetRotation;
    private Vector3 startRotation;

    private void Start()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
            
        startRotation = transform.localEulerAngles;
        targetRotation = Quaternion.Euler(startRotation);
    }

    private void Update()
    {
        // Suavemente retorna à rotação alvo
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, returnSpeed * Time.deltaTime);
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
                emission.rateOverTime = intensity * 10f;
            }
            else
            {
                noteParticles.Stop();
            }
        }
    }

    public void TiltToDirection(Vector2 direction)
    {
        // Calcula a rotação baseada na direção
        float tiltX = -direction.y * tiltAmount; // Inverte Y para cima/baixo fazer sentido
        float tiltZ = -direction.x * tiltAmount;
        
        Vector3 newRotation = startRotation + new Vector3(tiltX, 0, tiltZ);
        targetRotation = Quaternion.Euler(newRotation);
        
        // Aplica a rotação imediatamente com Lerp para suavizar
        transform.localRotation = Quaternion.Lerp(
            transform.localRotation,
            targetRotation,
            tiltSpeed * Time.deltaTime
        );
    }
}