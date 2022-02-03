using UnityEngine;
using UnityEngine.InputSystem;

/** This script is no longer used. Its functionality was moved to WeaponManager.cs */
public class PlayerShoot : MonoBehaviour
{
    public bool buttonDown = false;
    public WeaponManager weaponManager;

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
        {
            //weaponManager.SetAttack(true);
            //buttonDown = true;
        }
        else
        {
            //weaponManager.SetAttack(false);
        }
    }
}
