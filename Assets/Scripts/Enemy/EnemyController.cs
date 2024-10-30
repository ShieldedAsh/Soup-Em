using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyData stats;
    [SerializeField] private Transform targetTransform;

    private Vector3 targetPosition;
    private Rigidbody2D rb;

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
    }

    private void FixedUpdate()
    {
        Vector3 direction = targetTransform.position - transform.position;

        transform.position += stats.MoveSpeed * Time.deltaTime * direction.normalized;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                // Deal damage to player
                break;
        }
    }
}
