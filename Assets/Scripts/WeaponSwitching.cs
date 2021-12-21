using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwitching : MonoBehaviour
{
    /** Set this within the editor for which weapon to spawn with */
    [SerializeField] public int defaultWeaponIndex;
    private int currentWeaponIndex = 0;
    private int numberOfWeapons = 2;
    private int nextWeaponIndex;
    public Transform[] weaponArray;
    private float switchWeaponInput;

    // Start is called before the first frame update
    void Start()
    {
        currentWeaponIndex = defaultWeaponIndex;
        nextWeaponIndex = defaultWeaponIndex;
        numberOfWeapons = weaponArray.Length;
        for (int i = 0; i < numberOfWeapons; i++)
        {
            if (i == defaultWeaponIndex)
            {
                weaponArray[i].gameObject.SetActive(true);
            } else
            {
                weaponArray[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnScroll(InputAction.CallbackContext context)
    {
        switchWeaponInput = context.ReadValue<float>();
        if (switchWeaponInput > 0)
        {
            nextWeaponIndex = (nextWeaponIndex + 1) % numberOfWeapons;
        }
        else if (switchWeaponInput < 0)
        {
            if (nextWeaponIndex <= 0)
            {
                nextWeaponIndex = numberOfWeapons - 1;
            }
            else
            {
                nextWeaponIndex--;
            }
        }
        /** Num key code for weapon switch, implement later when all weapons added.
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
          selectedWeapon = 0;
        }
        etc.
        */
        if (nextWeaponIndex != currentWeaponIndex)
        {
            SelectWeapon();
        }
    }

    /**
    // Update is called once per frame
    void Update()
    {
        float ScrollWheelInput = Input.GetAxis("Mouse ScrollWheel");
        if (ScrollWheelInput > 0)
        {
            nextWeaponIndex = (nextWeaponIndex + 1) % numberOfWeapons;
        } else if (ScrollWheelInput < 0)
        {
            if (nextWeaponIndex <= 0)
            {
                nextWeaponIndex = numberOfWeapons - 1;
            } else
            {
                nextWeaponIndex--;
            }
        }
        /** Num key code for weapon switch, implement later when all weapons added.
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
          selectedWeapon = 0;
        }
        etc.
        if (nextWeaponIndex != currentWeaponIndex)
        {
            SelectWeapon();
        }
    }
    */

    void SelectWeapon()
    {
        weaponArray[currentWeaponIndex].gameObject.SetActive(false);
        weaponArray[nextWeaponIndex].gameObject.SetActive(true);
        currentWeaponIndex = nextWeaponIndex;
    }
}
