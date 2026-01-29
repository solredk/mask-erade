using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaypointManager : MonoBehaviour
{
    // idea of this is that WaypointManager is an instance because it subscribes to Scene loading 
    // Allowing me to subscribe an autoload wayholders so i can dynamicly load off of the scenes Array of waypoints

    public static WaypointManager Instance { get; private set; }

    private WayPointHolder holder;

    public int CurrentIndex { get; private set; } = 0;
    public int MaxIndex { get; private set; } = 0;
    public Vector3 CurrentRespawnPosition { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadWaypointInformation();
    }

    public void LoadWaypointInformation()
    {
        holder = FindFirstObjectByType<WayPointHolder>();

        if (holder == null || holder.wayPoints == null || holder.wayPoints.Length == 0)
        {
            // i default to 0 on any issues 
            MaxIndex = 0;
            CurrentIndex = 0;
            CurrentRespawnPosition = Vector3.zero;
            return;
        }

        MaxIndex = holder.wayPoints.Length - 1;

        CurrentIndex = Mathf.Clamp(CurrentIndex, 0, MaxIndex);
        Transform spawn = holder.GetWayPoint(CurrentIndex);
        if (spawn != null)
        {
         CurrentRespawnPosition = spawn.position;
        }
        else
        {
            CurrentRespawnPosition = Vector3.zero;
        }

    }
    
    public void Claim(int index)
    {
        if (holder == null) return;

        if (index < CurrentIndex) return;

        CurrentIndex = Mathf.Clamp(index, 0, MaxIndex);
        Transform spawn = holder.GetWayPoint(CurrentIndex);
        if (spawn != null)
            CurrentRespawnPosition = spawn.position;
    }

    public void Respawn(Transform player)
    {
        player.position = CurrentRespawnPosition;
    }
}
