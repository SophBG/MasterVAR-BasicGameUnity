using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    public float rotationSpeed;

    public Transform combatLookAt;

    public GameObject thirdPersonCam;
    public GameObject combatCam;
    public GameObject topDownCam;

    public CameraStyle currentStyle;
    public enum CameraStyle
    {
        Basic,
        Combat,
        Topdown
    }

    [Header("Input Actions")]
    public InputActionAsset inputActions;
    private InputAction switchToBasicCamAction;
    private InputAction switchToCombatCamAction;
    private InputAction switchToTopdownCamAction;
    private InputAction moveAction;

    private void OnEnable()
    {
        // Enable the action map (assuming it's called "Player" or create a new one for camera)
        inputActions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        // Disable the actions
        inputActions.FindActionMap("Player").Disable();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Get references to input actions
        switchToBasicCamAction = inputActions.FindAction("SwitchToBasicCam");
        switchToCombatCamAction = inputActions.FindAction("SwitchToCombatCam");
        switchToTopdownCamAction = inputActions.FindAction("SwitchToTopdownCam");
        moveAction = inputActions.FindAction("Move");

        // Set up callback functions
        switchToBasicCamAction.performed += ctx => SwitchCameraStyle(CameraStyle.Basic);
        switchToCombatCamAction.performed += ctx => SwitchCameraStyle(CameraStyle.Combat);
        switchToTopdownCamAction.performed += ctx => SwitchCameraStyle(CameraStyle.Topdown);
    }

    private void Update()
    {
        // rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        // rotate player object
        if(currentStyle == CameraStyle.Basic || currentStyle == CameraStyle.Topdown)
        {
            Vector2 moveInput = moveAction.ReadValue<Vector2>();
            Vector3 inputDir = orientation.forward * moveInput.y + orientation.right * moveInput.x;

            if (inputDir != Vector3.zero)
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }
        else if(currentStyle == CameraStyle.Combat)
        {
            Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            orientation.forward = dirToCombatLookAt.normalized;
            playerObj.forward = dirToCombatLookAt.normalized;
        }
    }

    private void SwitchCameraStyle(CameraStyle newStyle)
    {
        combatCam.SetActive(false);
        thirdPersonCam.SetActive(false);
        topDownCam.SetActive(false);

        if (newStyle == CameraStyle.Basic) thirdPersonCam.SetActive(true);
        if (newStyle == CameraStyle.Combat) combatCam.SetActive(true);
        if (newStyle == CameraStyle.Topdown) topDownCam.SetActive(true);

        currentStyle = newStyle;
    }
}