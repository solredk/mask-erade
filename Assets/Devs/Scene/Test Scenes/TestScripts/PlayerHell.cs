using UnityEngine;

public class PlayerHell : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        CharacterController controller = other.GetComponent<CharacterController>();
        if (controller == null)
        {
            return;
        }   
        WaypointManager.Instance.Respawn(controller.transform);
    }
} 