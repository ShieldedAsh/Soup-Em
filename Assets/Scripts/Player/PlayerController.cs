using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //create a variable to hold the rigibody and sprite renderer
    public Rigidbody2D rb;
    private SpriteRenderer sr;

    //load in the player's stats
    [SerializeField] private PlayerStats stats;

    //reg movement vars
    private float moveSpeed;
    private static int currentHealth;
    public static int CurrentHealth { get { return currentHealth; } set { currentHealth = value; } }

    //dash vars
    public float dashForce;
    public float dashDuration;

    //initalize player controls, dashing and attacking bools to prevent spamming
    private PlayerControls controls;
    private bool isDashing;
    private bool isAttacking;

    //load projectile prefab and the offset of where the projectile will be launched from
    public ProjectileBehavior ProjectilePrefab;
    public Transform LaunchOffset;

    private void Awake()
    {
        //enable controls
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
        //load in the player's rigidbody and sprite renderer
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        //obtain move speed and health from the player's stats
        moveSpeed = stats.MoveSpeed;
        currentHealth = stats.MaxHealth;

        //enable sprite renderer
        gameObject.GetComponent<SpriteRenderer>().enabled = true;

        SetPlayerStats();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //prevent the player from spam dashing
        if (!isDashing)
        {
            Move(controls.Combat.Move.ReadValue<Vector2>());
        }
    }

    //move the player
    private void Move(Vector2 moveInput)
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    //attack method
    private void Attack(InputAction.CallbackContext ctx)
    {
        //prevent the player from spam attacking
        if (!isAttacking)
        {
            IEnumerator attackCoroutine = ShootProjecctile();
            StartCoroutine(attackCoroutine);
        }
    }

    private void Dash(InputAction.CallbackContext ctx)
    {
        //let the player dash if they are moving
        if (rb.linearVelocity != Vector2.zero && !isDashing)
        {
            IEnumerator dashCoroutine = PerformDash();
            StartCoroutine(dashCoroutine);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //remove one health if the player collides with a regular enemy
        if (collision.gameObject.tag == "Enemy")
        {
            currentHealth -= 1;
            StartCoroutine(colorSwitch());
        }
        //remove 2 health if the player collides with a strong enemy
        else if (collision.gameObject.tag == "StrongEnemy")
        {
            currentHealth -= 2;
            StartCoroutine(colorSwitch());
        }
        //remove 5 health if the player collides with a boss enemy
        else if (collision.gameObject.tag == "BossEnemy")
        {
            currentHealth -= 5;
            StartCoroutine(colorSwitch());
        }

        //if the player's health is 0, the player is dead
        if (currentHealth <= 0)
        {
            Debug.Log("Player dead");
            controls.Combat.Disable();

            sr.enabled = false;

            Time.timeScale = 0;
            SceneMan.instance.DeathUI.SetActive(true);
        }
    }

    /// <summary>
    /// Allows the player to take damage from projectiles (should be called in ProjectileBehavior)
    /// </summary>
    public void ProjectileDamage()
    {
        currentHealth -= 1;
        StartCoroutine(colorSwitch());

        if (currentHealth <= 0)
        {
            Debug.Log("Player dead");
            Destroy(gameObject);
        }
    }


    // This smells but eh, whatever
    // Coroutine to perform the dash
    private IEnumerator PerformDash()
    {
        isDashing = true;
        rb.linearVelocity *= 2;
        yield return new WaitForSeconds(dashDuration);
        rb.linearVelocity /= 2;
        isDashing = false;
    }

    // Coroutine to perform the attack
    // allows for firing of 4 projectiles, one in each cardinal direction
    private IEnumerator ShootProjecctile()
    {
        isAttacking = true;
        Vector2 launchPosition;
        launchPosition.y = transform.position.y + 1;
        launchPosition.x = transform.position.x;
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
        launchPosition.y = transform.position.y - 1;
        launchPosition.x = transform.position.x;

        ProjectileBehavior projectile2 = Instantiate(ProjectilePrefab, launchPosition, transform.rotation);
        launchPosition.y = transform.position.y;
        launchPosition.x = transform.position.x - 1;

        ProjectileBehavior projectile3 = Instantiate(ProjectilePrefab, launchPosition, transform.rotation);
        launchPosition.y = transform.position.y;
        launchPosition.x = transform.position.x + 1;

        ProjectileBehavior projectile4 = Instantiate(ProjectilePrefab, launchPosition, transform.rotation);

        projectile.SetTarget(Vector2.up);
        projectile2.SetTarget(Vector2.down);
        projectile3.SetTarget(Vector2.left);
        projectile4.SetTarget(Vector2.right);
        yield return new WaitForSeconds(.5f);
        isAttacking = false;
    }

    //switch the player's color to red for a second when they take damage
    private IEnumerator colorSwitch()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(1.0f);
        sr.color = Color.white;
    }

    /// <summary>
    /// Sets player stats back to their max values (i.e, health is back to its default value)
    /// </summary>
    private void SetPlayerStats()
    {
        foreach (Upgrade upgrade in stats.upgrades)
        {
            switch (upgrade.Type)
            {
                case UpgradeType.Health:
                    {
                        currentHealth += upgrade.Value;

                        //Debug.Log($"Current health: {currentHealth}");

                        break;
                    }
                case UpgradeType.Damage:
                    {
                        // DAMAGE UPGRADE LOGIC HERE
                        stats.ProjData.Damage += upgrade.Value;
                        break;
                    }
            }
        }
    }
}
