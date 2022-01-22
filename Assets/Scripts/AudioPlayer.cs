using UnityEngine;

/**
 * Manages audio played from player and weapons.
 */
public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource rocketShootSound;
    [SerializeField] private AudioSource machineGunShootSound;
    
    public void PlayRocketShootSound()
    {
        rocketShootSound.Play();
    }

    public void PlayMachineGunShootSound()
    {
        machineGunShootSound.Play();
    }

}
