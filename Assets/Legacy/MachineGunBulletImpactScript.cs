using UnityEngine;

public class MachineGunBulletImpactScript : MonoBehaviour
{
    private float damage = 2;
    private float knockbackForce = 1.75f;
    /** Set by machinegun when spawned */
    public Vector3 direction;
    [SerializeField] private GameObject bulletImpactEffect;

    /** Another way to do direction, but I could see problems happening with this
    void Awake()
    {
        direction = Vector3.Normalize(this.GetComponent<Rigidbody>().velocity);
    }
    */

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 contactPoint = collision.GetContact(0).point;
        GetComponent<MeshRenderer>().enabled = false;
        //Make bullet impact
        Destroy(this.gameObject.GetComponent<Rigidbody>());
        Instantiate(bulletImpactEffect, contactPoint, Quaternion.LookRotation(collision.GetContact(0).normal));
        /** has to be on player layer */
        if(collision.gameObject.layer == 7)
        {
            GameObject player = collision.gameObject;
            float multiplier = (player.GetComponent<PlayerHealth>().health / 25) + .5f;
            player.GetComponent<PlayerHealth>().TakeDamage(damage);
            player.GetComponent<Rigidbody>().AddForce(direction * multiplier * knockbackForce, ForceMode.Impulse);
        } else if (collision.gameObject.layer == 8)
        {
            GameObject target = collision.gameObject;
            float multiplier = (target.GetComponent<TargetHealthScript>().health / 25) + .5f;
            target.GetComponent<TargetHealthScript>().TakeDamage(damage);
            target.GetComponent<Rigidbody>().AddForce(direction * multiplier * knockbackForce, ForceMode.Impulse);
        }
        Destroy(this.gameObject);
    }
}
