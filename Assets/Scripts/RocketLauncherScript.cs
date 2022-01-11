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
    private float fireRate = 1;

    void Awake()
    {
        launcherTransform = transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        playerHud = player.GetComponentInChildren<PlayerHudScript>();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.action.triggered && this.gameObject.activeInHierarchy == true && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            if (playerHealth.rockets > 0)
            {
                playerHealth.rockets--;
                SpawnRocket();
                playerHud.SetRockets(playerHealth.rockets);
                //play shooting sound
                //play shooting animation
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
        launchedRocket.GetComponent<Rigidbody>().AddForce(launcherTransform.forward * -8.0f, ForceMode.Impulse);
        Physics.IgnoreCollision(launchedRocket.GetComponent<Collider>(), player.GetComponent<Collider>());
    }
}
