using UnityEngine;
using UnityEngine.UI;

public class AmmoDisplay : MonoBehaviour
{
    [SerializeField] private Text ammoText;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Text healthText;

    // Update is called once per frame
    void Update()
    {
        ammoText.text = "Ammo: ";
        healthText.text = "Health: " + playerHealth.health.ToString();
    }
}
