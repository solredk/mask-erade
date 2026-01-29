using UnityEngine;

public class WayPointScript : MonoBehaviour
{

    [SerializeField] public int index;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) { return; }
        WaypointManager.Instance.Claim(index);
    }

       
    void Start()
    { 
    }
    void Update()
    {
        
    }
}
