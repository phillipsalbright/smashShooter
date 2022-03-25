using UnityEngine;

public class MeleeWeaponScript : Weapon
{
    private readonly float damage = 10f;
    private readonly float range = 2.6f;
    private readonly float knockbackForce = 12;
    private Animator meleeAnimator;
    private readonly float attackRate = 1f;

    public override void Setup()
    {
        meleeAnimator = GetComponent<Animator>();
    }

    public override void Attack()
    {
        nextTimeToAttack = Time.time + 1f / attackRate;
        meleeAnimator.SetTrigger("Attack");
        RaycastHit hit;
        if (Physics.Raycast(firingPoint.transform.position, firingPoint.transform.forward, out hit, range))
        {
            PlayerHealth target = hit.transform.GetComponent<PlayerHealth>();
            float multiplier = 1;
            if (target != null)
            {
                target.TakeDamage(damage);
                multiplier = (target.health / 25) + 1f;
            } else if (hit.transform.GetComponent<TargetHealthScript>())
            {
                hit.transform.GetComponent<TargetHealthScript>().TakeDamage(damage);
                multiplier = (hit.transform.GetComponent<TargetHealthScript>().health / 25) + 1f;
            }
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * knockbackForce * multiplier, ForceMode.Impulse);
            }
        }
    }

    public override void NoLongerAttacking()
    {
        return;
    }
}
