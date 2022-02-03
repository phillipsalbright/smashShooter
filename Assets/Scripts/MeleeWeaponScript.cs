using UnityEngine;

public class MeleeWeaponScript : Weapon
{
    private float damage = 10f;
    private float range = 2.6f;
    private float knockbackForce = 12;
    private Animator meleeAnimator;
    private float nextTimeToAttack = 0f;
    private float attackRate = 1f;
    /** Position of the crosshair on screen, used to detect if the attack should "hit" or not */
    public GameObject raycastPosition;
    // Start is called before the first frame update
    void Awake()
    {
        meleeAnimator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (attacking && Time.time >= nextTimeToAttack && this.gameObject.activeInHierarchy == true)
        {
            Debug.Log("frogs1");
            nextTimeToAttack = Time.time + 1f / attackRate;
            Attack();
        }
    }

    /** old input system
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToAttack)
        {
            nextTimeToAttack = Time.time + 1f / attackRate;
            Attack();
        }
    }
    */

    public override void Attack()
    {
        Debug.Log("frogs");
        meleeAnimator.SetTrigger("Attack");
        RaycastHit hit;
        if (Physics.Raycast(raycastPosition.transform.position, raycastPosition.transform.forward, out hit, range))
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
}
