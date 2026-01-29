using UnityEngine;

public class FireNextLevel : MonoBehaviour
{
    [SerializeField] WaypointManager waypointManager;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) { return; }
        waypointManager.LoadNextLevel();
        Debug.Log("Fired changing scene");

    }
    void Start()
    {
        
    }

 
    void Update()
    {
        
    }
}
