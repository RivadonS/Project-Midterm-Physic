using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    public int maxHP = 100;
    public int currentHP;
    public int keyCount = 0;
    public int attackDamage = 20;

    [Header("Movement")]
    public float baseSpeed = 5f;
    [HideInInspector] public float currentSpeed;

    [Header("Aiming (Raycast)")]
    public LayerMask groundLayer;
    public float weaponRange = 50f;
    private Rigidbody rb;
    private Camera mainCam;
    private GameObject currentTarget;

    [Header("Inputs")]
    public InputAction moveAction;
    public InputAction aimAction;
    public InputAction shootAction;

    [Header("Level Exit")]
    public string nextLevelName = "Level 2";
    public int requiredKeysToExit = 1;

    void Awake()
    {
        Time.timeScale = 1f;

        rb = GetComponent<Rigidbody>();
        mainCam = Camera.main;

        currentHP = maxHP;
        currentSpeed = baseSpeed;
    }

    void OnEnable()
    {
        moveAction.Enable();
        aimAction.Enable();
        shootAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
        aimAction.Disable();
        shootAction.Disable();
    }

    void Update()
    {
        HandleAiming();

        if (shootAction.triggered)
        {
            Shoot();
        }
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 inputDir = moveAction.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(inputDir.x, 0, inputDir.y).normalized;
        rb.MovePosition(rb.position + moveDir * currentSpeed * Time.fixedDeltaTime);
    }

    private void HandleAiming()
    {
        Vector2 mousePosition = aimAction.ReadValue<Vector2>();
        Ray ray = mainCam.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit groundHit, 100f, groundLayer))
        {
            Vector3 targetPoint = groundHit.point;
            targetPoint.y = transform.position.y;
            transform.LookAt(targetPoint);
        }
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, weaponRange))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                currentTarget = hit.collider.gameObject;
                Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);
            }
            else
            {
                currentTarget = null;
                Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.darkGreen);
            }
        }
        else
        {
            currentTarget = null;
            Debug.DrawRay(transform.position, transform.forward * weaponRange, Color.white);
        }
    }

    private void Shoot()
    {
        if (currentTarget != null)
        {
            Enemy enemyScript = currentTarget.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(attackDamage);
                Debug.Log($"Hit {currentTarget.name} Damage: {attackDamage}");
            }
        }
        else
        {
            Debug.Log("Not Hit");
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0) Debug.Log("Player Game Over!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ExitDoor"))
        {
            if (keyCount >= requiredKeysToExit)
            {
                Debug.Log("Loading to next level....");
                SceneManager.LoadScene(nextLevelName);
            }
            else
            {
                Debug.Log($"Have {keyCount}/{requiredKeysToExit} keys");
            }
        }
    }
}