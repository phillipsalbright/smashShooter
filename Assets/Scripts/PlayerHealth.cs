using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

/**
 * Class that handles player health/death calculations and manages what happens on player spawn and death. Also calls the Gun when ammo Pickups are hit.
 * Can change value of health and ammo pickups here.
 */
public class PlayerHealth : MonoBehaviour
{
    private PlayerHudScript playerHud;
    [SerializeField] private GameObject wholePlayer;

    public bool isDead { get; protected set; }
    public float health { get; protected set; }
    public int bullets;
    private int maxBullets = 100;
    public int rockets;
    private int maxRockets = 50;
    public int livesLeft;
    public int playerNumber;

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
        inputGetter.DeactivateInput();
        gameManager = GameManager.instance;
        playerNumber = gameManager.PlayerHasJoined(wholePlayer);
        playerHud = GetComponentInChildren<PlayerHudScript>();
    }

    public void Setup()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        livesLeft = gameManager.matchSettings.startingLives;
        Transform initialSpawnPoint = gameManager.GetInitialSpawnPoint();
        transform.position = initialSpawnPoint.position;
        float xrotation = initialSpawnPoint.rotation.eulerAngles.x;
        float yrotation = initialSpawnPoint.rotation.eulerAngles.y;
        this.GetComponent<PlayerLook>().SetRotation(xrotation, yrotation);
        playerHud.SetLives(livesLeft);
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
        playerHud.SetHealth(health);
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
        for (int i = 0; i < models.Length; i++)
        {
            models[i].enabled = false;
        }
        weaponHolder.SetActive(false);
        livesLeft--;
        playerHud.ClearHud();
        if (livesLeft > 0)
        {
            StartCoroutine(Respawn());
        } else
        {
            playerHud.UpdateHudForDeath();
            PlaceAsSpectator();
            gameManager.PlayerDied();
        }
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
                    playerHud.SetHealth(health);
                }
                Destroy(other.gameObject);
                break;
            case 12:
                int newBullets = bullets + 20;
                if (newBullets > maxBullets)
                {
                    bullets = maxBullets;
                }
                else
                {
                    bullets = newBullets;
                    playerHud.SetBullets(bullets);
                }
                Destroy(other.gameObject);
                break;
            case 11:
                int newRockets = rockets + 5;
                if (newRockets > maxRockets)
                {
                    rockets = maxRockets;
                }
                else
                {
                    rockets = newRockets;
                    playerHud.SetRockets(rockets);
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
        playerHud.SetHealth(health);
        playerHud.SetRockets(rockets);
        playerHud.SetBullets(bullets);
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

    private void PlaceAsSpectator()
    {
        Transform spectatorPoint = gameManager.GetSpectatorPoint();
        transform.position = spectatorPoint.position;
        this.GetComponent<PlayerLook>().SetRotation(spectatorPoint.rotation.eulerAngles.x, spectatorPoint.rotation.eulerAngles.y);
    }

    IEnumerator Respawn()
    {
        playerHud.SetLives(livesLeft);
        yield return new WaitForSeconds(1f);
        PlaceAsSpectator();
        yield return new WaitForSeconds(gameManager.matchSettings.respawnTime);
        Transform spawnPoint = gameManager.GetSpawnPoint();
        SetDefaults();
        transform.position = spawnPoint.position;
        float xrotation = spawnPoint.rotation.eulerAngles.x;
        float yrotation = spawnPoint.rotation.eulerAngles.y;
        this.GetComponent<PlayerLook>().SetRotation(xrotation, yrotation);
        for (int i = 0; i < models.Length; i++) {
            models[i].enabled = true;
        }
        weaponHolder.SetActive(true);
    }
}
