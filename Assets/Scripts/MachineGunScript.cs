using UnityEngine;

public class MachineGunScript : Weapon
{
    public GameObject bullet;
    /** Set to player holding this weapon */
    [SerializeField] private GameObject player;
    private PlayerHealth playerHealth;
    private PlayerHudScript playerHud;
    private Transform machineGunTransform;
    private float bulletSpeed = 32.0f;
    public float maxAmmo = 100;
    private float fireRate = 9;
    private float nextTimeToFire;
    [SerializeField] private Animator animator;
    private AudioPlayer audioPlayer;

    void Awake()
    {
        machineGunTransform = transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        playerHud = player.GetComponentInChildren<PlayerHudScript>();
        audioPlayer = GetComponentInParent<AudioPlayer>();
    }

    void FixedUpdate()
    {
        if (attacking && this.gameObject.activeInHierarchy == true && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            if (playerHealth.bullets > 0)
            {
                if (animator.GetBool("Shooting") != true)
                {
                    animator.SetBool("Shooting", true);
                }
                playerHealth.bullets--;
                Attack();
                playerHud.SetBullets(playerHealth.bullets);
                audioPlayer.PlayMachineGunShootSound();
            }
            else
            {
                animator.SetBool("Shooting", false);
                //play out of ammo sound
            }
        } else if (!attacking)
        {
            animator.SetBool("Shooting", false);
        }
    }

    public override void Attack()
    {
        Vector3 v = machineGunTransform.rotation.eulerAngles;
        GameObject shotBullet = Instantiate(bullet, machineGunTransform.transform.TransformPoint(0, .025f, -.2139f), Quaternion.Euler(v.x + 180f, v.y, v.z));
        shotBullet.GetComponent<Rigidbody>().AddForce(machineGunTransform.forward * -bulletSpeed, ForceMode.Impulse);
        shotBullet.GetComponent<MachineGunBulletImpactScript>().direction = machineGunTransform.forward * -1f;
        Physics.IgnoreCollision(shotBullet.GetComponent<Collider>(), player.GetComponent<Collider>());
    }
}
