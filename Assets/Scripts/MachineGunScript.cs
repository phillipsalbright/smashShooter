using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class MachineGunScript : MonoBehaviour
{
    public GameObject bullet;
    /** Set to player holding this weapon */
    [SerializeField] private GameObject player;
    private PlayerHealth playerHealth;
    private PlayerHudScript playerHud;
    private Transform machineGunTransform;
    private float bulletSpeed = 20.0f;
    public float maxAmmo = 100;
    private bool buttonDown = false;
    private float fireRate = 5;
    private float nextTimeToFire;
    private Animator animator;
    private AudioSource shootSound;

    // Start is called before the first frame update
    void Awake()
    {
        machineGunTransform = transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        playerHud = player.GetComponentInChildren<PlayerHudScript>();
        animator = GetComponent<Animator>();
        shootSound = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (buttonDown && this.gameObject.activeInHierarchy == true && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            if (playerHealth.bullets > 0)
            {
                playerHealth.bullets--;
                SpawnBullet();
                animator.SetBool("Shoot", true);
                playerHud.SetBullets(playerHealth.bullets);
                shootSound.Play();
            }
            else
            {
                animator.SetBool("Shoot", false);
                //play out of ammo sound
            }
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.action.triggered && this.gameObject.activeInHierarchy == true)
        {
            buttonDown = true;
            //StartCoroutine(Shooting());
        } else
        {
            buttonDown = false;
        }
    }
    /**
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            SpawnBullet();
        }
    }
    */
    /** old not used
    IEnumerator Shooting()
    {
        while (buttonDown && this.gameObject.activeInHierarchy == true) {
            if (playerHealth.bullets > 0)
            {
                playerHealth.bullets--;
                animator.Play("MachineGunShoot");
                SpawnBullet();
                playerHud.SetBullets(playerHealth.bullets);
                // play shoot sound
            } else
            {
                //play out of ammo sound
            }
            yield return new WaitForSeconds(1f / fireRate);
        }
    }
    */

    void SpawnBullet()
    {
        Vector3 v = machineGunTransform.rotation.eulerAngles;
        GameObject shotBullet = Instantiate(bullet, machineGunTransform.transform.TransformPoint(0, .025f, -.2139f), Quaternion.Euler(v.x + 180f, v.y, v.z));
        shotBullet.GetComponent<Rigidbody>().AddForce(machineGunTransform.forward * -bulletSpeed, ForceMode.Impulse);
        shotBullet.GetComponent<MachineGunBulletImpactScript>().direction = machineGunTransform.forward * -1f;
        Physics.IgnoreCollision(shotBullet.GetComponent<Collider>(), player.GetComponent<Collider>());
    }
}
