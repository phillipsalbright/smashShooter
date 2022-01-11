using UnityEngine;

public class RocketExplosionScript : MonoBehaviour
{
    private float blastRadius = 5;
    private float explosionForce = 25;
    /** Number of damage that will be inflicted for each hit */
    private float baseDamage = 8;
    /** Number of damage that will be inflicted variably depending on how far away the person is */
    private float variableDamage = 35;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private LayerMask players;

    private void OnCollisionEnter(Collision collision)
    {
        GetComponent<MeshRenderer>().enabled = false;
        Destroy(this.gameObject.GetComponent<Rigidbody>());
        Vector3 explosionPoint = collision.GetContact(0).point;
        Instantiate(explosionEffect, explosionPoint, Quaternion.identity);
        Collider[] hitColliders = Physics.OverlapSphere(explosionPoint, blastRadius, players);
        
        foreach(Collider hitcol in hitColliders)
        {
            float distance = (explosionPoint - hitcol.transform.position).magnitude;
            if (distance < 1)
            {
                distance = 1;
            }
            float multiplier = 1f;
            if (hitcol.GetComponent<PlayerHealth>())
            {
                hitcol.GetComponent<PlayerHealth>().TakeDamage(baseDamage + Mathf.RoundToInt(variableDamage / distance));
                multiplier = (hitcol.GetComponent<PlayerHealth>().health / 25) + .1f;
            } else if (hitcol.GetComponent<TargetHealthScript>())
            {
                hitcol.GetComponent<TargetHealthScript>().TakeDamage(baseDamage + Mathf.RoundToInt(variableDamage / distance));
                multiplier = (hitcol.GetComponent<TargetHealthScript>().health / 25) + .1f;
            }
            hitcol.GetComponent<Rigidbody>().AddExplosionForce(multiplier * explosionForce, explosionPoint, blastRadius, 1, ForceMode.Impulse);
        }
        Destroy(this.gameObject);
    }
}
