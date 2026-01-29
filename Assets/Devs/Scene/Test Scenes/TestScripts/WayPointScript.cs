using UnityEngine;

public class WayPointScript : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) { return; }
    }
    void Start()
    {
            
    }

    void Update()
    {
        
    }
}
