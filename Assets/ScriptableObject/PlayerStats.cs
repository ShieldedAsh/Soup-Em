using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int damagePerShot;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int money;
    
    public List<Upgrade> upgrades;

    public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
    public int Damage { get { return damagePerShot; } set { damagePerShot = value; } }
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
    public int Money { get { return money; } set { money = value; } }
}
