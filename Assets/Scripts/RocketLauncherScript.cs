using UnityEngine;

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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            SpawnRocket();
        }
    }

    void SpawnRocket()
    {
        GameObject launchedRocket = Instantiate(rocket, launcherTransform.transform.TransformPoint(0, 0, 0), launcherTransform.rotation);
        launchedRocket.GetComponent<Rigidbody>().AddForce(launcherTransform.forward * -7.0f, ForceMode.Impulse);
        Physics.IgnoreCollision(launchedRocket.GetComponent<Collider>(), player.GetComponent<Collider>());
    }
}
