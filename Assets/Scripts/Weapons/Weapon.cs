using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public bool attacking = false;
    public abstract void Attack();
    public int ammo;
    public float nextTimeToAttack;
    public abstract void NoLongerAttacking();
    public int maxAmmo;
    public Transform firingPoint;
    public abstract void Setup();
}
