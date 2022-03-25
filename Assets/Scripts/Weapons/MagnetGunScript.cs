using UnityEngine;
using VolumetricLines;

public class MagnetGunScript : Weapon
{
    /** Set to player holding this weapon */
    [SerializeField] private GameObject player;
    private readonly float fireRate = 22;
    [SerializeField] private Animator animator;
    [SerializeField] private LineRenderer laser;
    private AudioPlayer audioPlayer;
    private readonly float range = 30;
    private Vector3[] positions = new Vector3[2];
    private readonly float damage = 1;
    private readonly float force = 1;
    private readonly float hitwidth = .01f;
    private readonly float normwidth = .0034f;
    private bool activeLaser;
    private bool attackTime;

    public override void Setup()
    {
        maxAmmo = 150;
        audioPlayer = GetComponentInParent<AudioPlayer>();
    }

    private void FixedUpdate()
    {
        if (activeLaser)
        {
            laser.enabled = true;
            RaycastHit hit;
            Physics.Raycast(firingPoint.transform.position, firingPoint.transform.forward, out hit, range);
            PlayerHealth target = null;
            TargetHealthScript nonPlayer = null;
            if (hit.transform)
            {
                target = hit.transform.GetComponent<PlayerHealth>();
                nonPlayer = hit.transform.GetComponent<TargetHealthScript>();
            }
            if (target != null || nonPlayer != null)
            {
                positions[0] = firingPoint.position;
                positions[1] = hit.point;
                laser.SetPositions(positions);
                laser.startWidth = hitwidth;
                laser.endWidth = hitwidth;
                if (animator.GetBool("Shooting") != true)
                {
                    animator.SetBool("Shooting", true);
                    //audioPlayer.PlayMachineGunShootSound(); Play laser shooting sound
                    // set all other sounds off
                }
                float multiplier = 1;
                if (target != null)
                {
                    if (attackTime)
                    {
                        target.TakeDamage(damage);
                        ammo--;
                        attackTime = false;
                    }
                    multiplier = (target.health / 50) + .1f;
                }
                else if (nonPlayer != null)
                {
                    if (attackTime)
                    {
                        nonPlayer.TakeDamage(damage);
                        ammo--;
                        attackTime = false;
                    }
                    multiplier = (nonPlayer.health / 50) + .1f;
                }
                hit.rigidbody.AddForce(hit.normal * force * multiplier, ForceMode.Impulse );
            }
            else
            {
                Ray ray = new Ray(firingPoint.transform.position, firingPoint.transform.forward);
                positions[0] = firingPoint.position;
                positions[1] = ray.GetPoint(range);
                laser.SetPositions(positions);
                laser.startWidth = normwidth;
                laser.endWidth = normwidth;
                animator.SetBool("Shooting", false);

                //audioPlayer.PlayMachineGunShootSound(); turn this sound off

                // play laser NOT shooting sound
            }
        }
    }

    public override void Attack()
    {
        nextTimeToAttack = Time.time + 1f / fireRate;
        if (ammo > 0)
        {
            activeLaser = true;
            attackTime = true;
        } else
        {
            activeLaser = false;
            animator.SetBool("Shooting", false);
            laser.enabled = false;
            //set all other sounds off
        }



    }
    public override void NoLongerAttacking()
    {
        activeLaser = false;
        animator.SetBool("Shooting", false);
        laser.enabled = false;
        //Set all sounds off
    }
}
