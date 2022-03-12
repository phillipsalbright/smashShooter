using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBlastGunScript : Weapon
{
    public GameObject airBlast;
    /** Set to player holding this weapon */
    [SerializeField] private GameObject player;
    private Transform gunTransform;
    private readonly float fireRate = .9f;
    private readonly float projectileSpeed = 12;
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
            GameObject launchedProjectile = Instantiate(airBlast, gunTransform.transform.TransformPoint(0, 0, 0), gunTransform.rotation);
            launchedProjectile.GetComponent<Rigidbody>().AddForce(gunTransform.forward * -projectileSpeed, ForceMode.Impulse);
            launchedProjectile.GetComponent<AirBlast>().direction = gunTransform.forward * -1;
            Physics.IgnoreCollision(launchedProjectile.GetComponent<Collider>(), player.GetComponent<Collider>());
            //animator.SetTrigger("ShootRocket");
            //audioPlayer.PlayAirBlastShootSound();
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
