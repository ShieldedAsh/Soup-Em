//pretty much the same as the player controller, as the boss shoots projectiles
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BossController : MonoBehaviour
{
    [SerializeField] private EnemyData stats;
    [SerializeField] private Transform targetTransform;

    private Vector3 targetPosition;
    private Rigidbody2D rb;
    private bool canMove;
    private SpriteRenderer sr;
    private int currentHealth;

    private bool isAttacking;
    public ProjectileBehavior ProjectilePrefab;
    public Transform LaunchOffset;

    private void Start()
    {
        currentHealth = 50;

        if (targetPosition == null)
        {
            // if a target cannot be found, destroy this enemy instantly so it does not become a vegetable
            Debug.Log($"{gameObject.name}: Target not found!");
            Destroy(gameObject);
            return;
        }

        rb = gameObject.GetComponent<Rigidbody2D>();
        canMove = true;

        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        Vector3 direction = targetTransform.position - transform.position;

        if (canMove)
        {
            transform.position += stats.MoveSpeed * Time.deltaTime * direction.normalized;
        }

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Enemy dead");
        }

        Attack();
    }

    public void RemoveHealth(int value)
    {
        Debug.Log("been hit");
        currentHealth -= value;
        StartCoroutine(colorSwitch());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().CurrentHealth -= stats.AttackDamage;
        }
    }

    private void OnDestroy()
    {

    }

    private void Attack()
    {
        if (!isAttacking)
        {
            IEnumerator attackCoroutine = ShootProjecctile();
            StartCoroutine(attackCoroutine);
        }
    }

    private IEnumerator ShootProjecctile()
    {
        isAttacking = true;
        Vector2 launchPosition;
        launchPosition.y = transform.position.y + 12f;
        launchPosition.x = transform.position.x;

        ProjectileBehavior projectile = Instantiate(ProjectilePrefab, launchPosition, transform.rotation);
        launchPosition.y = transform.position.y - 12f;
        launchPosition.x = transform.position.x;

        ProjectileBehavior projectile2 = Instantiate(ProjectilePrefab, launchPosition, transform.rotation);
        launchPosition.y = transform.position.y;
        launchPosition.x = transform.position.x - 12f;

        ProjectileBehavior projectile3 = Instantiate(ProjectilePrefab, launchPosition, transform.rotation);
        launchPosition.y = transform.position.y;
        launchPosition.x = transform.position.x + 12f;

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
}
