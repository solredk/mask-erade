using UnityEngine;
using UnityEngine.Events;

public class WaypointManager : MonoBehaviour
{
    public static WaypointManager Instance { get; private set; }

    [SerializeField] private WayPointHolder holder;
    [SerializeField] private UnityEvent OnWayPointUnlocked;
    [SerializeField] private UnityEvent OnRespawn;

    public int CurrentIndex { get; private set; } = 0;
    public int MaxIndex { get; private set; } = 0;
    public Vector3 CurrentRespawnPosition { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (holder == null || holder.wayPoints == null || holder.wayPoints.Length == 0)
        {
            MaxIndex = 0;
            CurrentIndex = 0;
            CurrentRespawnPosition = Vector3.zero;
            return;
        }

        MaxIndex = holder.wayPoints.Length - 1;
        CurrentIndex = 0;

        Transform spawn = holder.GetWayPoint(CurrentIndex);
        CurrentRespawnPosition = spawn != null ? spawn.position : Vector3.zero;
    }

    public void Claim(int index)
    {
        if (holder == null) return;
        if (index <= CurrentIndex) return;

        CurrentIndex = Mathf.Clamp(index, 0, MaxIndex);

        OnWayPointUnlocked?.Invoke();

        Transform spawn = holder.GetWayPoint(CurrentIndex);
        if (spawn != null)
        {
            CurrentRespawnPosition = spawn.position;
        }
    }

    public void Respawn(Transform player)
    {
        CharacterController controller = player.GetComponent<CharacterController>();

        if (controller != null)
        {
            OnRespawn?.Invoke();
            controller.enabled = false;
            player.position = CurrentRespawnPosition;
            controller.enabled = true;
        }
    }
}