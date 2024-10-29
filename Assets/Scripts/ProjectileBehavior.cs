using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    [SerializeField] private ProjectileData data;
    private Vector3 direction;
    
    // Update is called once per frame
    void Update()
    {
        transform.position += data.Speed * Time.deltaTime * direction.normalized;
    }
    
    /// <summary>
    /// Sets target of projectile based off of movement input
    /// </summary>
    /// <param name="input">A 2D-Vector representing directional input from the player</param>
    public void SetTarget(Vector2 input)
    {
        if (input != Vector2.zero)
        {
            Vector3 targetVector = transform.position + (Vector3) input;
            direction = targetVector - transform.position;
        }
        else
        {
            direction = Vector2.up;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
