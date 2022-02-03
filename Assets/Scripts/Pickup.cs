using System.Collections;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private GameObject pickupObject;

    public void usePickup()
    {
        pickupObject.SetActive(false);
        StartCoroutine(RespawnPickup());
    }

    IEnumerator RespawnPickup()
    {
        yield return new WaitForSeconds(30);
        pickupObject.SetActive(true);
    }
}
