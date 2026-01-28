using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] public BatMask batmask;


    void Start()
    {
        
    }
    void Update()
    {
        
    }

    public void EquipBat(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            
        }
    }
}
