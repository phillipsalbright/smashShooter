using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    public bool buttonDown = false;

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
        {
            buttonDown = true;
        }
        else
        {
            buttonDown = false;
        }
    }
}
