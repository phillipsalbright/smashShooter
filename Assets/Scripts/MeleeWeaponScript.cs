using UnityEngine;

public class MeleeWeaponScript : MonoBehaviour
{
    private float damage = 10f;
    private float range = 4;
    private float knockbackForce = 12;
    private Animator meleeAnimator;
    private float nextTimeToAttack = 0f;
    private float attackRate = 1f;
    /** Position of the crosshair on screen, used to detect if the attack should "hit" or not */
    public GameObject raycastPosition;
    // Start is called before the first frame update
    void Start()
    {
        meleeAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToAttack)
        {
            nextTimeToAttack = Time.time + 1f / attackRate;
            Attack();
        }
    }

    private void Attack()
    {
        meleeAnimator.SetTrigger("Attack");
        RaycastHit hit;
        if (Physics.Raycast(raycastPosition.transform.position, raycastPosition.transform.forward, out hit, range))
        {
            PlayerHealth target = hit.transform.GetComponent<PlayerHealth>();
            if (target != null)
            {
                Debug.Log("frog1");
                target.TakeDamage(damage);
            }
            if (hit.rigidbody != null)
            {
                Debug.Log("frog2");
                float multiplier = (target.health / 25) + 1f;
                hit.rigidbody.AddForce(-hit.normal * knockbackForce, ForceMode.Impulse);
            }
        }
    }
}
