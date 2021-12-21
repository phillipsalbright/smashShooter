using UnityEngine;
using UnityEngine.InputSystem;

public class RocketLauncherScript : MonoBehaviour
{
    public GameObject rocket;
    /** Set to player holding this weapon */
    [SerializeField] private GameObject player;
    private Transform launcherTransform;
    // Start is called before the first frame update
    void Start()
    {
        launcherTransform = transform;
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.action.triggered && this.gameObject.activeInHierarchy == true)
        {
            SpawnRocket();
        }
    }

    /**
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            SpawnRocket();
        }
    }
    */

    void SpawnRocket()
    {
        GameObject launchedRocket = Instantiate(rocket, launcherTransform.transform.TransformPoint(0, 0, 0), launcherTransform.rotation);
        launchedRocket.GetComponent<Rigidbody>().AddForce(launcherTransform.forward * -8.0f, ForceMode.Impulse);
        Physics.IgnoreCollision(launchedRocket.GetComponent<Collider>(), player.GetComponent<Collider>());
    }
}
