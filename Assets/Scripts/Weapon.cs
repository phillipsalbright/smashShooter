using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public bool attacking = false;
    public abstract void Attack();
}
