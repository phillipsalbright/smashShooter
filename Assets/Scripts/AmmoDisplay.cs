using UnityEngine;
using UnityEngine.UI;

public class AmmoDisplay : MonoBehaviour
{
    [SerializeField] private Text ammoText;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Text healthText;
    [SerializeField] private Text bulletText;
    [SerializeField] private Text rocketText;

    /**
    void Update()
    {
        ammoText.text = "Ammo: ";
        healthText.text = "Health: " + playerHealth.health.ToString();
    }
    */

    public void setHealth(float health)
    {
        healthText.text = "Health: " + health;
    }

    public void setBullets(int bullets)
    {
        bulletText.text = "B: " + bullets;
    }

    public void setRockets(int rockets)
    {
        rocketText.text = "R: " + rockets;
    }
}
