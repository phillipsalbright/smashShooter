using UnityEngine;

public class MeleeWeaponScript : MonoBehaviour
{
    private float damage = 10f;
    private float range = 10f;
    private float knockbackForce = 10f;
    private Animator meleeAnimator;
    private float nextTimeToAttack = 0f;
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

        }
    }

    private void Attack()
    {
        
    }
}
