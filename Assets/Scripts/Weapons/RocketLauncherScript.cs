using UnityEngine;

public class RocketLauncherScript : Weapon
{
    public GameObject rocket;
    /** Set to player holding this weapon */
    [SerializeField] private GameObject player;
    private Transform launcherTransform;
    private readonly float fireRate = .9f;
    private readonly float projectileSpeed = 12;
    [SerializeField] private Animator animator;
    private AudioPlayer audioPlayer;

    void Awake()
    {
        maxAmmo = 20;
        launcherTransform = transform;
        audioPlayer = GetComponentInParent<AudioPlayer>();
    }

    public override void Attack()
    {
        nextTimeToAttack = Time.time + 1f / fireRate;
        if (ammo > 0)
        {
            ammo--;
            GameObject launchedRocket = Instantiate(rocket, launcherTransform.transform.TransformPoint(0, 0, 0), launcherTransform.rotation);
            launchedRocket.GetComponent<Rigidbody>().AddForce(launcherTransform.forward * -projectileSpeed, ForceMode.Impulse);
            Physics.IgnoreCollision(launchedRocket.GetComponent<Collider>(), player.GetComponent<Collider>());
            animator.SetTrigger("ShootRocket");
            audioPlayer.PlayRocketShootSound();
        }
        else
        {
            //play not shooting sound
        }
    }

    public override void NoLongerAttacking()
    {
        return;
    }
}
