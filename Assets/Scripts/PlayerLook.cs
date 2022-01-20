using UnityEngine;
using UnityEngine.InputSystem;

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

    /** Set to true if a controller is detected, used to make sure controller and mouse logic separate */
    private bool controller = false;


    /** Set these for initial rotation values in editor */
    public float xRotation;
    public float yRotation;

    /** Orientation of the player in the scene, gameobject attached to player keeping track of this */
    /** If player hitbox is not symmetrical on x/z plane, would have to change to rotate whole player */
    [SerializeField] private Transform orientation;

    public void SetupLook()
    {
        sensitivity = PlayerPrefs.GetFloat("Sensitivity" + this.gameObject.GetComponent<PlayerHealth>().playerNumber, .5f) * 2f;
        sensitivityX = 1;
        sensitivityY = 1;
        if (sensitivity <= 0)
        {
            sensitivity = .1f;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (controller || context.control.displayName == "Right Stick")
        {
            mouseX = context.ReadValue<Vector2>().x;
            mouseY = context.ReadValue<Vector2>().y;
            controller = true;
        } else
        {
            mouseX = context.ReadValue<Vector2>().x / 60;
            mouseY = context.ReadValue<Vector2>().y / 60;
            yRotation += mouseX * sensitivityX * sensitivity;
            xRotation -= mouseY * sensitivityY * sensitivity;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            mainCamera.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }
    
    void Update()
    {
        /**
        //GetInput();
    
        mainCamera.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);
        */
        if (controller)
        {
            yRotation += mouseX * sensitivityX * sensitivity;
            xRotation -= mouseY * sensitivityY * sensitivity;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            mainCamera.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }

    /** Used by other scripts to rotate the player */
    public void SetRotation(float x, float y)
    {
        xRotation = x;
        yRotation = y;
        mainCamera.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
    /** Old input system
    private void GetInput()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");
        yRotation += mouseX * sensitivityX * sensitivity;
        xRotation -= mouseY * sensitivityY * sensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    }
    */
}
