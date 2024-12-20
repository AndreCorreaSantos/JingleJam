using UnityEngine;

public class EffectObject : MonoBehaviour
{

    [SerializeField] private float lifeTime = 1f;
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {

    }
}
