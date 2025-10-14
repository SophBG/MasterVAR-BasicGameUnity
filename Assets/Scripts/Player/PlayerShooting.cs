using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerShooting : MonoBehaviour
{
    [System.Serializable]
    public class Weapon
    {
        public string name;
        public float bulletSpeed;           // Speed of bullet movement
        public float fireRate;              // Time between shots
        public float bulletDamage;          // Damage dealt per bullet
        public bool isAuto;                 // Automatic or semi-auto firing mode
        public GameObject bulletPrefab;     // Reference to bullet prefab
    }

    [Header("Weapons")]
    public Weapon[] weapons = new Weapon[3];    // Array for 3 weapons
    public int currentWeaponIndex = 0;          // Currently selected weapon

    [Header("Initial Setup")]
    public Transform bulletSpawnTransform;  // Position where bullets spawn

    [Header("Input Actions")]
    public InputActionAsset inputActions;   // Input actions asset
    private InputAction shootAction;        // Shoot input action
    private InputAction gun1Action;         // Switch to gun 1
    private InputAction gun2Action;         // Switch to gun 2
    private InputAction gun3Action;         // Switch to gun 3

    [Header("Events")]
    public UnityEvent<Weapon> NewWeapon;

    private float timer;        // Cooldown timer for shooting
    private bool isShooting;    // Track if shoot button is held

    // Current weapon properties for easy access
    private Weapon CurrentWeapon => weapons[currentWeaponIndex];
    private float BulletSpeed => CurrentWeapon.bulletSpeed;
    private float FireRate => CurrentWeapon.fireRate;
    private float BulletDamage => CurrentWeapon.bulletDamage;
    private bool IsAuto => CurrentWeapon.isAuto;
    private GameObject BulletPrefab => CurrentWeapon.bulletPrefab;

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
        // Clean up input action subscriptions
        if (shootAction != null)
        {
            shootAction.performed -= OnShootPerformed;
            shootAction.canceled -= OnShootCanceled;
        }
        
        if (gun1Action != null) gun1Action.performed -= OnGun1Performed;
        if (gun2Action != null) gun2Action.performed -= OnGun2Performed;
        if (gun3Action != null) gun3Action.performed -= OnGun3Performed;
    }

    private void Start()
    {
        // Get references to input actions
        shootAction = inputActions.FindAction("Shoot");
        gun1Action = inputActions.FindAction("Gun1");
        gun2Action = inputActions.FindAction("Gun2");
        gun3Action = inputActions.FindAction("Gun3");

        // Set up callbacks
        shootAction.performed += OnShootPerformed;
        shootAction.canceled += OnShootCanceled;
        
        gun1Action.performed += OnGun1Performed;
        gun2Action.performed += OnGun2Performed;
        gun3Action.performed += OnGun3Performed;

        // Initialize first weapon
        SwitchWeapon(0);
    }

    private void Update()
    {
        // Update cooldown timer
        if (timer > 0)
            timer -= Time.deltaTime;
        
        // Handle auto fire when button is held
        if (IsAuto && isShooting && timer <= 0)
        {
            Shoot();
        }
    }

    private void OnShootPerformed(InputAction.CallbackContext context)
    {
        // Handle shoot input based on firing mode
        if (timer <= 0)
        {
            if (IsAuto)
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
        if (IsAuto)
        {
            isShooting = false;
        }
    }

    private void OnGun1Performed(InputAction.CallbackContext context)
    {
        SwitchWeapon(0);
    }

    private void OnGun2Performed(InputAction.CallbackContext context)
    {
        SwitchWeapon(1);
    }

    private void OnGun3Performed(InputAction.CallbackContext context)
    {
        SwitchWeapon(2);
    }

    private void SwitchWeapon(int newWeaponIndex)
    {
        if (newWeaponIndex >= 0 && newWeaponIndex < weapons.Length && weapons[newWeaponIndex].bulletPrefab != null)
        {
            currentWeaponIndex = newWeaponIndex;
            NewWeapon?.Invoke(weapons[currentWeaponIndex]);
            
            // Reset shooting state when switching weapons
            isShooting = false;
        }
    }

    private void Shoot()
    {
        // Check if current weapon is valid
        if (BulletPrefab == null)
        {
            Debug.LogWarning($"No bullet prefab assigned for weapon {currentWeaponIndex}!");
            return;
        }

        // Create bullet and set its properties
        GameObject bullet = Instantiate(BulletPrefab, bulletSpawnTransform.position, bulletSpawnTransform.rotation, GameObject.FindGameObjectWithTag("WorldObjectHolder").transform);
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawnTransform.forward * BulletSpeed, ForceMode.Impulse);
        bullet.GetComponent<Projectile>().Initialize(BulletDamage);

        timer = 1f / FireRate; // Reset cooldown timer
    }

    // Public method to modify weapons at runtime
    public void UpdateWeapon(int index, Weapon newWeapon)
    {
        if (index >= 0 && index < weapons.Length)
        {
            weapons[index] = newWeapon;
            if (index == currentWeaponIndex)
            {
                // If updating current weapon, reset timer
                timer = 0;
            }
        }
    }
}