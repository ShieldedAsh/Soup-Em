using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //create a variable to hold the rigibody and sprite renderer
    public Rigidbody2D rb;
    private SpriteRenderer sr;

    //load in the player's stats
    [SerializeField] private PlayerStats stats;
    [SerializeField] private float knockbackCoeffecient;

    //reg movement vars
    private float moveSpeed;
    public int CurrentHealth { get { return stats.CurrentHealth; } set { stats.CurrentHealth = value; } }

    //dash vars
    public float dashForce;
    public float dashDuration;

    //initalize player controls, dashing and attacking bools to prevent spamming
    private PlayerControls controls;
    private bool isDashing;
    private bool isAttacking;
    private bool canMove;
    private bool isPaused;

    //load projectile prefab and the offset of where the projectile will be launched from
    public ProjectileBehavior ProjectilePrefab;
    public Transform LaunchOffset;

    private void Awake()
    {
        //enable controls
        controls = InputManager.controls;
        controls.Combat.Enable();

        canMove = true;

        controls.Combat.Dash.performed += Dash;
        isDashing = false;

        controls.Combat.Attack.performed += Attack;
        isAttacking = false;

        controls.Combat.Pause.performed += PauseGame;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //load in the player's rigidbody and sprite renderer
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        //obtain move speed and health from the player's stats
        moveSpeed = stats.MoveSpeed;

        UIManager.instance.UpdateHealthUI();

        //enable sprite renderer
        sr.enabled = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //prevent the player from spam dashing
        if (canMove)
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

            if (rb != null)
            {
                StartCoroutine(dashCoroutine);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //remove one health if the player collides with a regular enemy
        if (collision.gameObject.tag == "Enemy")
        {
            rb.AddForce(collision.gameObject.GetComponent<EnemyController>().Directon * knockbackCoeffecient, ForceMode2D.Impulse);
            stats.CurrentHealth -= 1;
            StartCoroutine(colorSwitch());
            StartCoroutine(LockControls(.25f));
        }
        //remove 2 health if the player collides with a strong enemy
        else if (collision.gameObject.tag == "StrongEnemy")
        {
            rb.AddForce(collision.gameObject.GetComponent<EnemyController>().Directon * knockbackCoeffecient, ForceMode2D.Impulse);
            stats.CurrentHealth -= 2;
            StartCoroutine(colorSwitch());
            StartCoroutine(LockControls(.25f));
        }
        //remove 5 health if the player collides with a boss enemy
        else if (collision.gameObject.tag == "BossEnemy")
        {
            rb.AddForce(collision.gameObject.GetComponent<EnemyController>().Directon * knockbackCoeffecient, ForceMode2D.Impulse);
            stats.CurrentHealth -= 5;
            StartCoroutine(colorSwitch());
            StartCoroutine(LockControls(.25f));
        }

        UIManager.instance.UpdateHealthUI();

        //if the player's health is 0, the player is dead
        if (stats.CurrentHealth <= 0)
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
        stats.CurrentHealth -= 1;
        StartCoroutine(colorSwitch());

        if (stats.CurrentHealth <= 0)
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

    public void AddMoney(int amount)
    {
        stats.Money += amount;
    }

    private void OnApplicationQuit()
    {
        stats.upgrades.Clear();
        stats.Money = 0;
    }

    private void PauseGame(InputAction.CallbackContext ctx)
    {
        if (!isPaused)
        {
            Time.timeScale = 0;
            isPaused = true;
        }
        else
        {
            Time.timeScale = 1;
            isPaused = false;
        }
    }

    private IEnumerator LockControls(float duration)
    {
        canMove = false;
        controls.Combat.Disable();
        yield return new WaitForSeconds(duration);
        controls.Combat.Enable();
        canMove = true;
    }
}
