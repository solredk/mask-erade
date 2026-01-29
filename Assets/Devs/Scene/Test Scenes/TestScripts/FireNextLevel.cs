using UnityEngine;

public class FireNextLevel : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) { return; }
        GameManager.GameInstance.LoadNextLevel();
        Debug.Log("Fired changing scene");

    }
    void Start()
    {
        
    }

 
    void Update()
    {
        
    }
}
