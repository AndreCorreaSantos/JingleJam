using UnityEngine;

public class EffectObject : MonoBehaviour
{

    [SerializeField] private float lifeTime = 1f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, lifeTime);
    }
}
