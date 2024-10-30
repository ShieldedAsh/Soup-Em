using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "SoupEm/Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int moveSpeed;
    [SerializeField] private int attackDamage;
    // Reward on death todo after initial demo

    private int currentHealth;

    public int CurrentHealth { get { return currentHealth; } set { currentHealth = value; } }
    public int MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
    public int AttackDamage { get { return attackDamage; } set { attackDamage = value; } }

    private void Awake()
    {
        currentHealth = maxHealth;
    }
}
