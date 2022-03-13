using UnityEngine;
using UnityEngine.UI;

public class PlayerHudScript : MonoBehaviour
{
    [SerializeField] private Text healthText;
    [SerializeField] private Text bulletText;
    [SerializeField] private Text rocketText;
    [SerializeField] private Text livesText;
    [SerializeField] private Text deathText;

    /**
    void Update()
    {
        ammoText.text = "Ammo: ";
        healthText.text = "Health: " + playerHealth.health.ToString();
    }
    */

    public void SetHealth(float health)
    {
        healthText.text = "Health: " + health;
    }

    public void SetBlaster(int blasts)
    {
        bulletText.text = "B: " + blasts;
    }

    public void SetRockets(int rockets)
    {
        rocketText.text = "R: " + rockets;
    }

    public void SetLives(int lives)
    {
        livesText.text = "Lives: " + lives;
    }

    public void ClearHud()
    {
        healthText.text = "";
        bulletText.text = "";
        rocketText.text = "";
        livesText.text = "";
    }

    public void UpdateHudForDeath()
    {
        ClearHud();
        deathText.gameObject.SetActive(true);
    }

    public void UndoDeathHud()
    {
        deathText.gameObject.SetActive(false);
    }
}
