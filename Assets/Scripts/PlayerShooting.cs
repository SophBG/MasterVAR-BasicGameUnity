using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [Header("Bullet Variables")]
    public float bulletSpeed;   // Speed of bullet movement
    public float fireRate;      // Time between shots
    public float bulletDamage;  // Damage dealt per bullet
    public bool isAuto;         // Automatic or semi-auto firing mode

    [Header("Initial Setup")]
    public Transform bulletSpawnTransform;  // Position where bullets spawn
    public GameObject bulletPrefab;         // Reference to bullet prefab

    [Header("Input Actions")]
    public InputActionAsset inputActions;   // Input actions asset
    private InputAction shootAction;        // Shoot input action

    private float timer;        // Cooldown timer for shooting
    private bool isShooting;    // Track if shoot button is held

    private void OnEnable()
    {
        // Enable input actions
        inputActions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        // Disable input actions
        inputActions.FindActionMap("Player").Disable();
    }

    private void OnDestroy()
    {
        // Clean up input action subscription
        if (shootAction != null)
            shootAction.performed -= OnShootPerformed;
            shootAction.canceled -= OnShootCanceled;
    }

    private void Start()
    {
        // Get reference to shoot action and set up callback
        shootAction = inputActions.FindAction("Shoot");
        shootAction.performed += OnShootPerformed;
        shootAction.canceled += OnShootCanceled;
    }

    private void Update()
    {
        // Update cooldown timer
        if (timer > 0)
            timer -= Time.deltaTime / fireRate;
        
        // Handle auto fire when button is held
        if (isAuto && isShooting && timer <= 0)
        {
            Shoot();
        }
    }

    private void OnShootPerformed(InputAction.CallbackContext context)
    {
        // Handle shoot input based on firing mode
        if (timer <= 0)
        {
            if (isAuto)
            {
                // For auto mode, start holding
                isShooting = true;
            }
            else
            {
                // For semi-auto, shoot once per press
                Shoot();
            }
        }
    }

    private void OnShootCanceled(InputAction.CallbackContext context)
    {
        // Stop auto firing when button is released
        if (isAuto)
        {
            isShooting = false;
        }
    }

    private void Shoot()
    {
        // Create bullet and set its properties
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnTransform.position, bulletSpawnTransform.rotation, GameObject.FindGameObjectWithTag("WorldObjectHolder").transform);
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawnTransform.forward * bulletSpeed, ForceMode.Impulse);
        bullet.GetComponent<Projectile>().damage = bulletDamage;

        timer = 1; // Reset cooldown timer
    }
}