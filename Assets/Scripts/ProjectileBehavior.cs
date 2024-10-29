using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    [SerializeField] private ProjectileData data;

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * data.Speed * Time.deltaTime;
    }
    
    /// <summary>
    /// Sets directiopn of porojectile based off of movement input
    /// </summary>
    /// <param name="input">A 2D-Vector representing directional input from the player</param>
    public void SetDirection(Vector2 input)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
