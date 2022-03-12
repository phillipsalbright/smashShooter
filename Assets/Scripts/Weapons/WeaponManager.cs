using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    /** Set this within the editor for which weapon to spawn with */
    [SerializeField] public int defaultWeaponIndex;
    private int currentWeaponIndex = 0;
    public Weapon currentWeapon;
    private int numberOfWeapons = 3;
    private int nextWeaponIndex;
    public Weapon[] weaponArray;
    private float switchWeaponInput;
    private bool attacking = false;
    [SerializeField] private PlayerHudScript playerHud;

    // Start is called before the first frame update
    void Start()
    {
        attacking = false;
        currentWeaponIndex = defaultWeaponIndex;
        nextWeaponIndex = defaultWeaponIndex;
        numberOfWeapons = weaponArray.Length;
        for (int i = 0; i < numberOfWeapons; i++)
        {
            if (i == defaultWeaponIndex)
            {
                weaponArray[i].gameObject.SetActive(true);
                currentWeapon = weaponArray[i];
            } else
            {
                weaponArray[i].gameObject.SetActive(false);
                weaponArray[i].attacking = false;
            }
        }
    }

    public void SetWeaponDefaults()
    {
        attacking = false;
        weaponArray[2].ammo = GameManager.instance.matchSettings.startingRockets;
        weaponArray[1].ammo = GameManager.instance.matchSettings.startingBullets;
        playerHud.SetRockets(weaponArray[2].ammo);
        playerHud.SetBullets(weaponArray[1].ammo);
    }

    private void FixedUpdate()
    {
        if (attacking && currentWeapon.gameObject.activeInHierarchy == true && Time.time >= currentWeapon.nextTimeToAttack)
        {
            currentWeapon.Attack();
            switch (currentWeaponIndex)
            {
                case 1:
                    playerHud.SetBullets(currentWeapon.ammo);
                    break;
                case 2:
                    playerHud.SetRockets(currentWeapon.ammo);
                    break;
                default:
                    break;
            }
        }
        else if (!attacking)
        {
            currentWeapon.NoLongerAttacking();
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

    void SelectWeapon()
    {
        weaponArray[currentWeaponIndex].gameObject.SetActive(false);
        weaponArray[nextWeaponIndex].attacking = false;
        weaponArray[nextWeaponIndex].gameObject.SetActive(true);
        currentWeapon = weaponArray[nextWeaponIndex];
        weaponArray[currentWeaponIndex].attacking = false;
        currentWeaponIndex = nextWeaponIndex;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            attacking = true;
        }
        else
        {
            attacking = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.layer)
        {
            case 11:
                other.gameObject.GetComponentInParent<Pickup>().usePickup();
                int newRockets = weaponArray[2].ammo + 4;
                if (newRockets > weaponArray[2].maxAmmo)
                {
                    weaponArray[2].ammo = weaponArray[2].maxAmmo;
                }
                else
                {
                    weaponArray[2].ammo = newRockets;
                }
                playerHud.SetRockets(weaponArray[2].ammo);
                break;
            case 12:
                other.gameObject.GetComponentInParent<Pickup>().usePickup();
                int newBullets = weaponArray[1].ammo + 20;
                if (newBullets > weaponArray[1].maxAmmo)
                {
                    weaponArray[1].ammo = weaponArray[1].maxAmmo;
                }
                else
                {
                    weaponArray[1].ammo = newBullets;
                }
                playerHud.SetBullets(weaponArray[1].ammo);
                break;
            default:
                break;
        }
    }
}
