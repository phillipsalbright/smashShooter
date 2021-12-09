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
        Vector3 v = machineGunTransform.rotation.eulerAngles;
        GameObject shotBullet = Instantiate(bullet, machineGunTransform.transform.TransformPoint(0, .025f, -.2139f), Quaternion.Euler(v.x + 180f, v.y, v.z));
        shotBullet.GetComponent<Rigidbody>().AddForce(machineGunTransform.forward * -7.0f, ForceMode.Impulse);
        Physics.IgnoreCollision(shotBullet.GetComponent<Collider>(), player.GetComponent<Collider>());
    }
}
