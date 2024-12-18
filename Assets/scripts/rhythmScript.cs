using UnityEngine;
using System.Collections;
public class rhythmScript : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _particleSystem.Play();
        _particleSystem.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        parseInput();
    }


    private void parseInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(PlayParticleSystem());
        }
    }
    private IEnumerator PlayParticleSystem()
    {
        _particleSystem.Play();
        yield return new WaitForSeconds(0.1f);
        _particleSystem.Stop();
    }
}
