using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    /** X axis sensitivity to be set in editor */
    private float sensitivityX;

    /** Y axis sensitivity to be set in editor */
    private float sensitivityY;

    /** Camera holder prefab within the scene */
    public Transform mainCamera;

    private float mouseX;

    private float mouseY;

    private float sensitivity;


    /** Set these for initial rotation values in editor */
    public float xRotation;
    public float yRotation;

    /** Orientation of the player in the scene, gameobject attached to player keeping track of this */
    /** If player hitbox is not symmetrical on x/z plane, would have to change to rotate whole player */
    [SerializeField] private Transform orientation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        sensitivity = PlayerPrefs.GetFloat("Sensitivity", .5f) * 2f;
        sensitivityX = 1;
        sensitivityY = 1;
        if (sensitivity <= 0)
        {
            sensitivity = 1;
        }
    }

    void Update()
    {
        GetInput();
        mainCamera.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void GetInput()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");
        yRotation += mouseX * sensitivityX * sensitivity;
        xRotation -= mouseY * sensitivityY * sensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    }
}
