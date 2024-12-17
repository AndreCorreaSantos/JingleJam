using UnityEngine;

public class rhythmScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the 'Q' key was pressed
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("The 'Q' key was pressed.");
            // Add your action here
        }
    }
}
