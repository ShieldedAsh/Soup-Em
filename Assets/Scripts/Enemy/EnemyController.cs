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
        //set current health to the max health at the start of the level
        currentHealth = stats.MaxHealth;

        if (targetTransform.position == null)
        {
            // if a target cannot be found, destroy this enemy instantly so it does not become a vegetable
            Debug.Log($"{gameObject.name}: Target not found!");
            Destroy(gameObject);
            return;
        }

        //obtain the rigidbody and sprite renderer of the enemy
        rb = gameObject.GetComponent<Rigidbody2D>();
        canMove = true;
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        //create a raycast to detect the player
        Vector3 direction = targetTransform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, visionRange, visionLayerMask);

        //if the player is detected, move towards the player
        if (hit)
        {
            if (hit.collider.gameObject.CompareTag("Player") && canMove)
            {
                transform.position += stats.MoveSpeed * Time.deltaTime * direction.normalized;
            }
        }

        //if the enemy's health is 0 or less, destroy the enemy
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Enemy dead");
        }
    }

    //method called when a projectile hits the enemy (it removes health from the enemy)
    public void RemoveHealth(int value)
    {
        currentHealth -= value;
        StartCoroutine(colorSwitch());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if the enemy collides with the player, remove health from the player
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

    //switch the color of the enemy when damaged
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
