using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "SoupEm/Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private int maxHealth;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int attackDamage;
    [SerializeField] private int moneyOnDeath;
    // Reward on death todo after initial demo

    public float MoveSpeed { get { return moveSpeed; } }
    public int AttackDamage { get { return attackDamage; }}
    public int MaxHealth { get { return maxHealth; } }
    public int MoneyOnDeath { get { return moneyOnDeath; } }
}
