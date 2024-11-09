using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "SoupEm/Scriptable Objects/ProjectileData")]
public class ProjectileData : ScriptableObject
{
    [SerializeField] private int damage;
    [SerializeField] private float projectileSpeed;

    public int Damage { get { return damage; } set { damage = value; } }
    public float Speed { get { return projectileSpeed; } }
}
