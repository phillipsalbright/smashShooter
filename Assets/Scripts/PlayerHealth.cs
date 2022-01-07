using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

/**
 * Class that handles player health/death calculations. Also calls the Gun when ammo Pickups are hit.
 * Can change value of health and ammo pickups here.
 */
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private AmmoDisplay ammoDisplay;

    public bool isDead { get; protected set; }
    public float health { get; protected set; }
    public int bullets;
    private int maxBullets = 100;
    public int rockets;
    private int maxRockets = 50;

    /** Used in death/respawn, mostly things that should be disabled and renabled in the process. */
    [SerializeField]
    private Behaviour[] disableOnDeath;
    [SerializeField]
    private PlayerInput inputGetter;
    [SerializeField]
    private MeshRenderer[] models;
    [SerializeField]
    private GameObject weaponHolder;
    private bool[] wasEnabled;

    //public DeathScreen deathScreen;
    public GameObject hurtImage;

    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
        transform.position = gameManager.startPoint.position;
        float xrotation = gameManager.startPoint.rotation.eulerAngles.x;
        float yrotation = gameManager.startPoint.rotation.eulerAngles.y;
        this.GetComponent<PlayerLook>().SetRotation(xrotation, yrotation);
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
        ammoDisplay.setHealth(health);
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
        for (int i = 0; i < models.Length; i++)
        {
            models[i].enabled = false;
        }
        weaponHolder.SetActive(false);
        StartCoroutine(Respawn());
        //deathScreen.Death();
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.layer)
        {
            case 10:
                /** Health pack */
                float newHealth = health - 10f;
                if (newHealth <= 0)
                {
                    health = 0;
                }
                else
                {
                    health = newHealth;
                    ammoDisplay.setHealth(health);
                }
                Destroy(other.gameObject);
                break;
            case 11:
                int newBullets = bullets + 20;
                if (newBullets > maxBullets)
                {
                    bullets = maxBullets;
                }
                else
                {
                    bullets = newBullets;
                    ammoDisplay.setBullets(bullets);
                }
                Destroy(other.gameObject);
                break;
            case 12:
                int newRockets = rockets + 5;
                if (newRockets > maxRockets)
                {
                    rockets = maxRockets;
                }
                else
                {
                    rockets = newRockets;
                    ammoDisplay.setRockets(rockets);
                }
                Destroy(other.gameObject);
                break;
            case 6:
                Death();
                break;
            default:
                break;
        }
    }

    private void SetDefaults()
    {
        isDead = false;
        health = gameManager.matchSettings.startingHealth;
        bullets = gameManager.matchSettings.startingBullets;
        rockets = gameManager.matchSettings.startingRockets;
        ammoDisplay.setHealth(health);
        ammoDisplay.setRockets(rockets);
        ammoDisplay.setBullets(bullets);
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
        yield return new WaitForSeconds(1f);
        transform.position = gameManager.spectatorPoint.position;
        this.GetComponent<PlayerLook>().SetRotation(gameManager.spectatorPoint.rotation.eulerAngles.x, gameManager.spectatorPoint.rotation.eulerAngles.y);
        yield return new WaitForSeconds(gameManager.matchSettings.respawnTime);
        int spawnPointIndex = Random.Range(0, gameManager.spawnPoints.Length);

        SetDefaults();
        transform.position = gameManager.spawnPoints[spawnPointIndex].position;
        float xrotation = gameManager.spawnPoints[spawnPointIndex].rotation.eulerAngles.x;
        float yrotation = gameManager.spawnPoints[spawnPointIndex].rotation.eulerAngles.y;
        this.GetComponent<PlayerLook>().SetRotation(xrotation, yrotation);
        for (int i = 0; i < models.Length; i++) {
            models[i].enabled = true;
        }
        weaponHolder.SetActive(true);
    }
}
