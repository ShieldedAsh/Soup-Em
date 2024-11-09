using Unity.VisualScripting;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    [SerializeField] private ProjectileData stats;
    private Vector3 direction;

    // Update is called once per frame
    private void FixedUpdate()
    {
        //move the projectile
        transform.position += stats.Speed * Time.deltaTime * direction.normalized;
    }

    /// <summary>
    /// Sets target of projectile based off of movement input
    /// </summary>
    /// <param name="input">A 2D-Vector representing directional input from the player</param>
    public void SetTarget(Vector2 input)
    {
        if (input != Vector2.zero)
        {
            Vector3 targetVector = transform.position + (Vector3)input;
            direction = targetVector - transform.position;
        }
        else
        {
            direction = Vector2.up;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //obtain the controller of the enemy or strong enemy and remove health from them
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "StrongEnemy")
        {
            collision.gameObject.GetComponent<EnemyController>().RemoveHealth(stats.Damage);
        }
        //obtain the controller of the boss enemy and remove health from it
        else if (collision.gameObject.tag == "BossEnemy")
        {
            collision.gameObject.GetComponent<BossController>().RemoveHealth(stats.Damage);
        }
        //allow the player to take damage from projectiles (the boss shoots projectiles)
        else if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().ProjectileDamage();
        }

        //after a collision, destroy the projectile
        Destroy(gameObject);

    }
}
