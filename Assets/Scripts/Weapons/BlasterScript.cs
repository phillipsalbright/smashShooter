using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlasterScript : Weapon
{
    public GameObject airBlast;
    /** Set to player holding this weapon */
    [SerializeField] private GameObject player;
    private Transform gunTransform;
    private readonly float fireRate = 2f;
    private readonly float projectileSpeed = 15;
    [SerializeField] private Animator animator;
    private AudioPlayer audioPlayer;

    void Awake()
    {
        maxAmmo = 50;
        gunTransform = transform;
        audioPlayer = GetComponentInParent<AudioPlayer>();
    }

    public override void Attack()
    {
        nextTimeToAttack = Time.time + 1f / fireRate;
        if (ammo > 0)
        {
            ammo--;
            GameObject launchedProjectile = Instantiate(airBlast, firingPoint.TransformPoint(0, 0, 0), firingPoint.rotation * Quaternion.Euler(90, 180, 0));
            launchedProjectile.GetComponent<Rigidbody>().AddForce(firingPoint.forward * projectileSpeed, ForceMode.Impulse);
            launchedProjectile.GetComponent<Blast>().direction = gunTransform.forward * -1;
            Physics.IgnoreCollision(launchedProjectile.GetComponent<Collider>(), player.GetComponent<Collider>());
            animator.SetTrigger("ShootBlaster");
            audioPlayer.PlayBlasterShootSound();
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
