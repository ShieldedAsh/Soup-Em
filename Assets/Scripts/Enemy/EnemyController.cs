using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyData stats;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float visionRange;
    [SerializeField] private LayerMask visionLayerMask;

    private Rigidbody2D rb;
    private bool canMove;
    private SpriteRenderer sr;
    private int currentHealth;

    private void Start()
    {
        currentHealth = stats.MaxHealth;

        if (targetTransform.position == null)
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, visionRange, visionLayerMask);

        if (hit)
        {
            if (hit.collider.gameObject.CompareTag("Player") && canMove)
            {
                transform.position += stats.MoveSpeed * Time.deltaTime * direction.normalized;
            }
        }

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Enemy dead");
        }
    }

    public void RemoveHealth(int value)
    {
        currentHealth -= value;
        StartCoroutine(colorSwitch());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController.CurrentHealth -= stats.AttackDamage;
        }

        Debug.Log("I died!");
    }

    private void OnDestroy()
    {
        // Debug.Log("FUcxk i died");
    }

    private IEnumerator colorSwitch()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(1.0f);
        if (gameObject.tag == "Enemy")
        {
            sr.color = Color.cyan;
        }
        else if (gameObject.tag == "StrongEnemy")
        {
            sr.color = Color.yellow;
        }
    }
}
