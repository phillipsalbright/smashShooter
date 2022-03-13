using UnityEngine;

public class MachineGunScript : Weapon
{
    public GameObject bullet;
    /** Set to player holding this weapon */
    [SerializeField] private GameObject player;
    private Transform machineGunTransform;
    private readonly float bulletSpeed = 32.0f;
    private readonly float fireRate = 9;
    [SerializeField] private Animator animator;
    private AudioPlayer audioPlayer;

    void Awake()
    {
        maxAmmo = 150;
        machineGunTransform = transform;
        audioPlayer = GetComponentInParent<AudioPlayer>();
    }

    public override void Attack()
    {
        nextTimeToAttack = Time.time + 1f / fireRate;
        if (ammo > 0)
        {
            if (animator.GetBool("Shooting") != true)
            {
                animator.SetBool("Shooting", true);
            }
            ammo--;
            Vector3 v = machineGunTransform.rotation.eulerAngles;
            GameObject shotBullet = Instantiate(bullet, machineGunTransform.transform.TransformPoint(0, .025f, -.2139f), Quaternion.Euler(v.x + 180f, v.y, v.z));
            shotBullet.GetComponent<Rigidbody>().AddForce(machineGunTransform.forward * -bulletSpeed, ForceMode.Impulse);
            shotBullet.GetComponent<MachineGunBulletImpactScript>().direction = machineGunTransform.forward * -1f;
            Physics.IgnoreCollision(shotBullet.GetComponent<Collider>(), player.GetComponent<Collider>());
            audioPlayer.PlayMachineGunShootSound();
        }
        else
        {
            animator.SetBool("Shooting", false);
            //play out of ammo sound
        }
        

    }
    public override void NoLongerAttacking()
    {
        animator.SetBool("Shooting", false);
    }
}
