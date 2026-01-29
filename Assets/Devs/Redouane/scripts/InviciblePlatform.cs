using UnityEngine;

public class InviciblePlatform : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    private Collider platformCollider;
    
    [SerializeField] private int invincibleLayer = 7;
    [SerializeField] private int visibleLayer = 6;
    private void Start()
    {
        platformCollider = gameObject.GetComponent<Collider>();        
    }


    private void Update()
    {
        if (inputManager.currentMask == Masks.bat && gameObject.layer != visibleLayer)
        {
            gameObject.layer = visibleLayer;
            platformCollider.enabled = true;
        }
        else if (inputManager.currentMask != Masks.bat && gameObject.layer != invincibleLayer)
        {
            gameObject.layer = invincibleLayer;
            platformCollider.enabled = false;
        }
    }
}
