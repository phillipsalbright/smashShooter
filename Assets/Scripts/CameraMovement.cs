using UnityEngine;

/**
 * Moves the first person camera from outside of the player to reduce jittery visuals.
 */
public class CameraMovement : MonoBehaviour
{
    /** Camera position attached to player */
    [SerializeField] private Transform cameraPosition;

    void Update()
    {
        transform.position = cameraPosition.position;
    }
}
