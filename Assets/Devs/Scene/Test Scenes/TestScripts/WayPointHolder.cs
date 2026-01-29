using UnityEngine;

public class WayPointHolder : MonoBehaviour
{
    public Transform[] wayPoints;
    private GameObject player;


    private void Awake()
    {
        

    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public Transform GetWayPoint(int index)
    {
        if (wayPoints == null || wayPoints.Length == 0) return null;
        index = Mathf.Clamp(index, 0, wayPoints.Length - 1);
        return wayPoints[index];    
    }
 


    void Update()
    {
        
    }
}

