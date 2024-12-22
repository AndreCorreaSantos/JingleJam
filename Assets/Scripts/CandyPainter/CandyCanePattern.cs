using UnityEngine;
using System.Collections.Generic;

public class CandyCanePattern : MonoBehaviour
{
    [Header("Pattern Settings")]
    [Range(1, 4)]
    [SerializeField] private int minActiveNotes = 1;
    [Range(1, 4)]
    [SerializeField] private int maxActiveNotes = 4;
    
    [Header("Pattern Chances")]
    [Range(0f, 1f)]
    [SerializeField] private float chanceForMaxNotes = 0.6f; // Chance de ter todas as notas ativas
    
    private void Start()
    {
        RandomizePattern();
    }

    public void RandomizePattern()
    {
        // Pega todos os indicadores de nota filhos
        List<GameObject> noteIndicators = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag("NoteIndicator"))
            {
                noteIndicators.Add(transform.GetChild(i).gameObject);
            }
        }

        // Decide quantas notas estarão ativas
        int notesToActivate;
        if (Random.value < chanceForMaxNotes)
        {
            notesToActivate = maxActiveNotes;
        }
        else
        {
            notesToActivate = Random.Range(minActiveNotes, maxActiveNotes + 1);
        }

        // Desativa todas as notas primeiro
        foreach (GameObject note in noteIndicators)
        {
            note.SetActive(false);
        }

        // Ativa um número aleatório de notas
        List<int> availableIndices = new List<int>();
        for (int i = 0; i < noteIndicators.Count; i++)
        {
            availableIndices.Add(i);
        }

        for (int i = 0; i < notesToActivate; i++)
        {
            if (availableIndices.Count == 0) break;
            
            int randomIndex = Random.Range(0, availableIndices.Count);
            int noteIndex = availableIndices[randomIndex];
            
            noteIndicators[noteIndex].SetActive(true);
            availableIndices.RemoveAt(randomIndex);
        }
    }

    // Opcional: Método para forçar um padrão específico
    public void SetSpecificPattern(bool[] pattern)
    {
        if (pattern.Length != transform.childCount)
        {
            Debug.LogWarning("Pattern length doesn't match number of note indicators!");
            return;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag("NoteIndicator"))
            {
                transform.GetChild(i).gameObject.SetActive(pattern[i]);
            }
        }
    }
}