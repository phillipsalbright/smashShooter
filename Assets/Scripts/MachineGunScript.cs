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
    [SerializeField] private Animator animator;
    private AudioPlayer audioPlayer;

    // Start is called before the first frame update
    void Awake()
    {
        machineGunTransform = transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        playerHud = player.GetComponentInChildren<PlayerHudScript>();
        audioPlayer = GetComponentInParent<AudioPlayer>();
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
                playerHud.SetBullets(playerHealth.bullets);
                audioPlayer.PlayMachineGunShootSound();
            }
            else
            {
                animator.SetBool("Shooting", false);
                //play out of ammo sound
            }
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (this.gameObject.activeInHierarchy == true)
        {
            if (context.action.triggered)
            {
                buttonDown = true;
                animator.SetBool("Shooting", true);
                //StartCoroutine(Shooting());
            } else
            {
                animator.SetBool("Shooting", false);
                buttonDown = false;
            }
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
