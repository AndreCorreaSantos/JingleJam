using System.Collections;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.VFX;

[ExecuteInEditMode]
public class VFXManager : MonoBehaviour
{
    private Mesh tMesh;
    private VisualEffect dissolveEffect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tMesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
        //get child component called dissolve
        Transform dTrans = transform.Find("dissolve");
        dTrans.position = transform.position;
        dissolveEffect = dTrans.gameObject.GetComponent<VisualEffect>();
        //set property to the dissolve effect dissolve.
        dissolveEffect.SetMesh("ObjectMesh", tMesh);
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
        // destroy the object
        Destroy(gameObject);
        
    }
}
