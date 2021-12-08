using UnityEngine;

public class MachineGunScript : MonoBehaviour
{
    public GameObject bullet;
    /** Set to player holding this weapon */
    [SerializeField] private GameObject player;
    private Transform machineGunTransform;
    // Start is called before the first frame update
    void Start()
    {
        machineGunTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            SpawnBullet();
        }
    }

    void SpawnBullet()
    {
        GameObject shotBullet = Instantiate(bullet, machineGunTransform.transform.TransformPoint(0, 0, 0), machineGunTransform.rotation);
        shotBullet.GetComponent<Rigidbody>().AddForce(machineGunTransform.forward * -7.0f, ForceMode.Impulse);
        Physics.IgnoreCollision(shotBullet.GetComponent<Collider>(), player.GetComponent<Collider>());
    }
}
