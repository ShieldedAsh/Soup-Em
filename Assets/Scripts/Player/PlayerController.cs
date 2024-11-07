using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    private SpriteRenderer sr;

    [SerializeField] private PlayerStats stats;

    //reg movement vars
    private float moveSpeed;
    private int currentHealth;

    public int CurrentHealth { get { return currentHealth; } set { currentHealth = value; } }

    //dash var
    public float dashForce;
    public float dashDuration;

    private PlayerControls controls;
    private bool isDashing;
    private bool isAttacking;

    public ProjectileBehavior ProjectilePrefab;
    public Transform LaunchOffset;

    private void Awake()
    {
        controls = InputManager.controls;
        controls.Combat.Enable();

        controls.Combat.Dash.performed += Dash;
        isDashing = false;

        controls.Combat.Attack.performed += Attack;
        isAttacking = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        moveSpeed = stats.MoveSpeed;
        currentHealth = stats.MaxHealth;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isDashing)
        {
            Move(controls.Combat.Move.ReadValue<Vector2>());
        }
    }

    private void Move(Vector2 moveInput)
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    private void Attack(InputAction.CallbackContext ctx)
    {
        if (!isAttacking)
        {
            IEnumerator attackCoroutine = ShootProjecctile();
            StartCoroutine(attackCoroutine);
        }
    }

    private void Dash(InputAction.CallbackContext ctx)
    {
        if (rb.linearVelocity != Vector2.zero && !isDashing)
        {
            IEnumerator dashCoroutine = PerformDash();
            StartCoroutine(dashCoroutine);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            currentHealth -= 1;
            StartCoroutine(colorSwitch());
        }
        else if (collision.gameObject.tag == "StrongEnemy")
        {
            currentHealth -= 2;
            StartCoroutine(colorSwitch());
        }

        if (currentHealth <= 0)
        {
            Debug.Log("Player dead");
            Destroy(gameObject);
        }
    }


    // This smells but eh, whatever
    private IEnumerator PerformDash()
    {
        isDashing = true;
        rb.linearVelocity *= 2;
        yield return new WaitForSeconds(dashDuration);
        rb.linearVelocity /= 2;
        isDashing = false;
    }

    private IEnumerator ShootProjecctile()
    {
        isAttacking = true;
        Vector2 launchPosition = transform.position;
        //Vector2 moveInput = controls.Combat.Attack.ReadValue<Vector2>();

        /*  if (moveInput != Vector2.zero)
        {
            launchPosition += moveInput;
        }
        else
        {
            launchPosition += Vector2.up;
        }*/

        ProjectileBehavior projectile = Instantiate(ProjectilePrefab, launchPosition, transform.rotation);
        ProjectileBehavior projectile2 = Instantiate(ProjectilePrefab, launchPosition, transform.rotation);
        ProjectileBehavior projectile3 = Instantiate(ProjectilePrefab, launchPosition, transform.rotation);
        ProjectileBehavior projectile4 = Instantiate(ProjectilePrefab, launchPosition, transform.rotation);
        projectile.SetTarget(Vector2.up);
        projectile2.SetTarget(Vector2.down);
        projectile3.SetTarget(Vector2.left);
        projectile4.SetTarget(Vector2.right);
        yield return new WaitForSeconds(.5f);
        isAttacking = false;
    }

    private IEnumerator colorSwitch()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(1.0f);
        sr.color = Color.white;
    }

    /// <summary>
    /// Sets player stats back to their max values (i.e, health is back to its default value)
    /// </summary>
    private void ResetPlayerStats()
    {
        foreach (Upgrade upgrade in stats.upgrades)
        {
            int upgradeValue = upgrade.Value * upgrade.Level;

            switch (upgrade.Type)
            {
                case Upgrade.UpgradeType.Health:
                    {
                        currentHealth = stats.MaxHealth + upgradeValue;
                        
                        //Debug.Log($"Current health: {currentHealth}");
                        
                        break;
                    }
                case Upgrade.UpgradeType.Damage:
                    {
                        // DAMAGE UPGRADE LOGIC HERE
                        break;
                    }
            }
        }
    }
}
