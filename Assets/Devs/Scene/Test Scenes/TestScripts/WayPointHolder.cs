using UnityEngine;

public class WayPointHolder : MonoBehaviour
{
    public Transform[] wayPoints;

    public Transform GetWayPoint(int index)
    {
        if (wayPoints == null || wayPoints.Length == 0) return null;
        index = Mathf.Clamp(index, 0, wayPoints.Length - 1);
        return wayPoints[index];    
    }
 
    void Start()
    {
        
    }


    void Update()
    {
        
    }
}

