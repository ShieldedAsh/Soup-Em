using Unity.VisualScripting;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    [SerializeField] private ProjectileData stats;
    private Vector3 direction;

    // Update is called once per frame
    private void FixedUpdate()
    {
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
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "StrongEnemy" 
            || collision.gameObject.tag == "BossEnemy")
        {
            collision.gameObject.GetComponent<EnemyController>().RemoveHealth(stats.Damage);
        }
        else if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().ProjectileDamage();
        }

        Destroy(gameObject);

    }
}
