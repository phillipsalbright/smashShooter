using UnityEngine;
using UnityEngine.InputSystem;

public class RocketLauncherScript : MonoBehaviour
{
    public GameObject rocket;
    /** Set to player holding this weapon */
    [SerializeField] private GameObject player;
    private PlayerHealth playerHealth;
    private PlayerHudScript playerHud;
    private Transform launcherTransform;

    public float maxAmmo = 20;
    private float nextTimeToFire = 0f;
    private float fireRate = .9f;
    private float projectileSpeed = 12;
    [SerializeField] private Animator animator;
    private AudioPlayer audioPlayer;
    [SerializeField] private PlayerShoot playerShoot;

    void Awake()
    {
        launcherTransform = transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        playerHud = player.GetComponentInChildren<PlayerHudScript>();
        audioPlayer = GetComponentInParent<AudioPlayer>();
    }

    void FixedUpdate()
    {
        if (playerShoot.buttonDown && this.gameObject.activeInHierarchy == true && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            if (playerHealth.rockets > 0)
            {
                playerHealth.rockets--;
                SpawnRocket();
                animator.SetTrigger("ShootRocket");
                playerHud.SetRockets(playerHealth.rockets);
                audioPlayer.PlayRocketShootSound();
            } else
            {
                //play not shooting sound
            }
        }
    }

    /**
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            SpawnRocket();
        }
    }
    */

    void SpawnRocket()
    {
        GameObject launchedRocket = Instantiate(rocket, launcherTransform.transform.TransformPoint(0, 0, 0), launcherTransform.rotation);
        launchedRocket.GetComponent<Rigidbody>().AddForce(launcherTransform.forward * -projectileSpeed, ForceMode.Impulse);
        Physics.IgnoreCollision(launchedRocket.GetComponent<Collider>(), player.GetComponent<Collider>());
    }
}
