using UnityEngine;
using System.Collections;

/**
 * Class that handles player health/death calculations. Also calls the Gun when ammo Pickups are hit.
 * Can change value of health and ammo pickups here.
 */
public class PlayerHealth : MonoBehaviour
{
    public float health;

    /** Set from playerWhole prefab. Need playerHud for hurt animations */
    //public DeathScreen deathScreen;
    public GameObject hurtImage;
    /** Set gun to this in editor */
    //[SerializeField] private Gun gun;
    //private float nextAcidDamage = 0;

    public void TakeDamage(float damage)
    {
        health += damage;
        StartCoroutine(HurtAnimation());
    }

    IEnumerator HurtAnimation()
    {
        hurtImage.SetActive(true);
        yield return new WaitForSeconds(.1f);
        hurtImage.SetActive(false);
    }

    void Death()
    {
        //deathScreen.Death();
    }

    private void OnTriggerEnter(Collider other)
    {
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
            */
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
            */
        }
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
}
