using UnityEngine;

/**
 * Class that handles player health/death calculations for a simple target.
 */
public class TargetHealthScript : MonoBehaviour
{
    public float health { get; protected set; }


    public void TakeDamage(float damage)
    {
        health += damage;
    }
}
