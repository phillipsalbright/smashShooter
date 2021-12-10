using UnityEngine;

public class RocketExplosionScript : MonoBehaviour
{
    private float blastRadius = 5;
    private float explosionForce = 30;
    private float damage = 5;
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
            float multiplier = (hitcol.GetComponent<PlayerHealth>().health / 25) + .1f;
            hitcol.GetComponent<PlayerHealth>().TakeDamage(damage);
            hitcol.GetComponent<Rigidbody>().AddExplosionForce(multiplier * explosionForce, explosionPoint, blastRadius, 1, ForceMode.Impulse);
        }
        Destroy(this.gameObject);
    }
}
