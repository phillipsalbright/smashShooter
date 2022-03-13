using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blast : MonoBehaviour
{
    private float force = 6;
    private float damage = 20;

    public Vector3 direction;

    public void Start()
    {
        StartCoroutine(LifeTime());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            GameObject player = other.gameObject;
            float multiplier = (player.GetComponent<PlayerHealth>().health / 22) + 1.5f;
            player.GetComponent<PlayerHealth>().TakeDamage(damage);
            player.GetComponent<Rigidbody>().AddForce(direction * multiplier * force, ForceMode.Impulse);
        }
        else if (other.gameObject.layer == 8)
        {
            GameObject target = other.gameObject;
            float multiplier = (target.GetComponent<TargetHealthScript>().health / 10) + 5f;

            target.GetComponent<TargetHealthScript>().TakeDamage(damage);
            target.GetComponent<Rigidbody>().AddForce(direction * multiplier * force, ForceMode.Impulse);
        } else if (other.gameObject.layer == 3)
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(1.2f);
        Destroy(this.gameObject);
    }
}
