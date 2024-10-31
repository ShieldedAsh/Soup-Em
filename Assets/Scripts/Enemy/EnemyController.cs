using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyData stats;
    [SerializeField] private Transform targetTransform;

    private Vector3 targetPosition;
    private Rigidbody2D rb;
    private bool canMove;

    private void Start()
    {
        if (targetPosition == null) 
        {
            // if a target cannot be found, destroy this enemy instantly so it does not become a vegetable
            Debug.Log($"{gameObject.name}: Target not found!");
            Destroy(gameObject);
            return;
        }

        rb = gameObject.GetComponent<Rigidbody2D>();
        canMove = true;
    }

    private void FixedUpdate()
    {
        Vector3 direction = targetTransform.position - transform.position;

        if (canMove)
        {
            transform.position += stats.MoveSpeed * Time.deltaTime * direction.normalized;
        }

        if (stats.CurrentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void RemoveHealth(int value)
    {
        stats.CurrentHealth -= value;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                collision.gameObject.GetComponent<PlayerController>().Health -= stats.AttackDamage;
                break;
        }
    }
}
