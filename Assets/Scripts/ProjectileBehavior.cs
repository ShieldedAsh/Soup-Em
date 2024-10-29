using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    [SerializeField] private ProjectileData data;

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * data.Speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
