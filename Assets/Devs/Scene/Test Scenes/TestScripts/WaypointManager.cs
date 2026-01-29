using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class WaypointManager : MonoBehaviour
{
    public static WaypointManager Instance { get; private set; }

    private WayPointHolder holder;
    [SerializeField] private UnityEvent OnWayPointUnlocked;
    [SerializeField] private UnityEvent OnRespawn;
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
        CurrentIndex = 0;
        LoadWaypointInformation();
    }

    public void LoadWaypointInformation()
    {


        holder = FindFirstObjectByType<WayPointHolder>();

        if (holder == null)
        {

            MaxIndex = 0;
            CurrentIndex = 0;
            CurrentRespawnPosition = Vector3.zero;
            return;
        }

        if (holder.wayPoints == null || holder.wayPoints.Length == 0)
        {

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
        if (holder == null)
        {
            return;
        }

        if (index < CurrentIndex)
        {
            return;
        }

        CurrentIndex = Mathf.Clamp(index, 0, MaxIndex);
        Debug.Log($"[WaypointManager] CurrentIndex updated to: {CurrentIndex}");

        OnWayPointUnlocked.Invoke();
        Transform spawn = holder.GetWayPoint(CurrentIndex);
        if (spawn != null)
        {
            CurrentRespawnPosition = spawn.position;
            Debug.Log($"[WaypointManager] Claimed waypoint {CurrentIndex}, position: {CurrentRespawnPosition}");
        }
        else
        {
            Debug.LogWarning($"[WaypointManager] Claim - spawn at index {CurrentIndex} is null");
        }
    }

    public void Respawn(Transform player)
    {
        CharacterController controller = player.GetComponent<CharacterController>();

        if (controller != null)
        {

            OnRespawn.Invoke();
            controller.enabled = false;
            player.position = CurrentRespawnPosition;

            controller.enabled = true;

        }
    }
}