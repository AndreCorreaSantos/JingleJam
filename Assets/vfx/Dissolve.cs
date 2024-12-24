using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class Dissolve : MonoBehaviour
{
    private Mesh tMesh;
    public VisualEffect dissolveEffect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dissolveEffect.enabled = false;
    }
    public void playEffect()
    {
        StartCoroutine(dissolve());
    }
    private IEnumerator dissolve()
    {
        //set the dissolve effect to active
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        dissolveEffect.enabled = true;
        yield return new WaitForSeconds(1.0f);
    }
}
