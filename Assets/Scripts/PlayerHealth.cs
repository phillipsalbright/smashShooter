using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

/**
 * Class that handles player health/death calculations. Also calls the Gun when ammo Pickups are hit.
 * Can change value of health and ammo pickups here.
 */
public class PlayerHealth : MonoBehaviour
{
    /**
     * This is code that would be needed if switched to an online game. Have to do this is you want to syncvar.
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }
    */

    public bool isDead { get; protected set; }
    public float health { get; protected set; }

    /** Used in death/respawn */
    [SerializeField]
    private Behaviour[] disableOnDeath;
    [SerializeField]
    private PlayerInput inputGetter;
    private bool[] wasEnabled;

    /** Set from playerWhole prefab. Need playerHud for hurt animations */
    //public DeathScreen deathScreen;
    public GameObject hurtImage;
    /** Set gun to this in editor */
    //[SerializeField] private Gun gun;
    //private float nextAcidDamage = 0;

    private void Start()
    {
        Setup();
    }

    public void Setup()
    {
        wasEnabled = new bool[disableOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }
        SetDefaults();
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
        {
            return;
        }
        health += damage;
        if (hurtImage != null)
        {
            StartCoroutine(HurtAnimation());
        }
    }

    IEnumerator HurtAnimation()
    {
        hurtImage.SetActive(true);
        yield return new WaitForSeconds(.1f);
        hurtImage.SetActive(false);
    }

    public void Death()
    {
        isDead = true;
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }
        if (inputGetter != null)
        {
            inputGetter.DeactivateInput();
        }
        Debug.Log("DEAD!");
        //Call Respawn method
        StartCoroutine(Respawn());
        //deathScreen.Death();
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.layer)
        {
            case 10:
                /** Health pack */
                float newHealth = health + 10f;
                if (newHealth >= 50f)
                {
                    health = 50f;
                }
                else
                {
                    health = newHealth;
                }
                Destroy(other.gameObject);
                break;
            case 11:
                /** Ammo pack, will need more of these with our more ammo types (this is from a single gun game */
                /**
                if (gun.ammoCount >= gun.maxAmmo)
                {
                    return;
                }
                int newAmmo = gun.ammoCount + 6;
                if (newAmmo > gun.maxAmmo)
                {
                    gun.ammoCount = gun.maxAmmo;
                } else
                {
                    gun.ammoCount = newAmmo;
                }
                Destroy(other.gameObject);
                */
                break;
            case 6:
                Death();
                break;
            default:
                break;
        }
        /** old stuff. I put most of this in the switch statement.
        if (other.gameObject.layer == 10)
        {
            float newHealth = health + 10f;
            if (newHealth >= 50f)
            {
                health = 50f;
            } else
            {
                health = newHealth;
            }
            Destroy(other.gameObject);
        }
        if (other.gameObject.layer == 11)
        {
            /**
            if (gun.ammoCount >= gun.maxAmmo)
            {
                return;
            }
            int newAmmo = gun.ammoCount + 6;
            if (newAmmo > gun.maxAmmo)
            {
                gun.ammoCount = gun.maxAmmo;
            } else
            {
                gun.ammoCount = newAmmo;
            }
            Destroy(other.gameObject);
        }
        if (other.gameObject.layer == 13)
        {
            //StartCoroutine(AcidCheck(other));
            /**
            while (other.bounds.Intersects(this.gameObject.GetComponentInChildren<CapsuleCollider>().bounds))
            {
                if (Time.time >= nextAcidDamage)
                {
                    TakeDamage(3f);
                    nextAcidDamage = Time.time + 2f;
                }
            }
        }
        */
    }
    /**
    IEnumerator AcidCheck(Collider other)
    {
        while (other.bounds.Intersects(this.gameObject.GetComponentInChildren<CapsuleCollider>().bounds))
        {
            if (Time.time >= nextAcidDamage)
            {
                TakeDamage(3f);
                nextAcidDamage = Time.time + 2f;
            }
            yield return new WaitForSeconds(2f);
        }
    }
    */
    private void SetDefaults()
    {
        isDead = false;
        health = 0;
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }
        if (inputGetter != null)
        {
            inputGetter.ActivateInput();
        }
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3f);
        SetDefaults();
        Vector3 spawnPoint = new Vector3(0, 10, 0);
        transform.position = spawnPoint;
        /** Go back and make a manager with spawn points for each map.
         * Use both spawnpoint position and rotation
         */
    }
}
