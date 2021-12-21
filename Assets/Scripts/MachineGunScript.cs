using UnityEngine;
using UnityEngine.InputSystem;

public class MachineGunScript : MonoBehaviour
{
    public GameObject bullet;
    /** Set to player holding this weapon */
    [SerializeField] private GameObject player;
    private Transform machineGunTransform;
    private float bulletSpeed = 12.0f;
    // Start is called before the first frame update
    void Awake()
    {
        machineGunTransform = transform;
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.action.triggered && this.gameObject.activeInHierarchy == true)
        {
            SpawnBullet();
        }
    }
    /**
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            SpawnBullet();
        }
    }
    */

    void SpawnBullet()
    {
        Vector3 v = machineGunTransform.rotation.eulerAngles;
        GameObject shotBullet = Instantiate(bullet, machineGunTransform.transform.TransformPoint(0, .025f, -.2139f), Quaternion.Euler(v.x + 180f, v.y, v.z));
        shotBullet.GetComponent<Rigidbody>().AddForce(machineGunTransform.forward * -bulletSpeed, ForceMode.Impulse);
        shotBullet.GetComponent<MachineGunBulletImpactScript>().direction = machineGunTransform.forward * -1f;
        Physics.IgnoreCollision(shotBullet.GetComponent<Collider>(), player.GetComponent<Collider>());
    }
}
