using UnityEngine;

public class CandyCaneConductor : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float tiltAmount = 20f;
    [SerializeField] private float tiltSpeed = 5f;
    [SerializeField] private float returnSpeed = 3f;
    
    private Quaternion targetRotation;
    private Vector3 startRotation;

    private void Start()
    {
        startRotation = transform.localEulerAngles;
        targetRotation = Quaternion.Euler(startRotation);
    }

    private void Update()
    {
        // Suavemente retorna à rotação alvo
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, returnSpeed * Time.deltaTime);
    }

    public void TiltToDirection(Vector2 direction)
    {
        // Calcula a rotação baseada na direção
        float tiltX = -direction.y * tiltAmount;
        float tiltZ = -direction.x * tiltAmount;
        
        Vector3 newRotation = startRotation + new Vector3(tiltX, 0, tiltZ);
        targetRotation = Quaternion.Euler(newRotation);
        
        transform.localRotation = Quaternion.Lerp(
            transform.localRotation,
            targetRotation,
            tiltSpeed * Time.deltaTime
        );
    }
}