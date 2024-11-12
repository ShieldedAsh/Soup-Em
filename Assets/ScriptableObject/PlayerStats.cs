using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [SerializeField] private int maxHealth;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int money;
    [SerializeField] private ProjectileData projectileData;
    
    public List<Upgrade> upgrades;

    public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
    public int Damage { get { return ProjData.Damage; } set { ProjData.Damage = value; } }
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
    public int Money { get { return money; } set { money = value; } }
    public ProjectileData ProjData { get { return projectileData; } set { projectileData = value; } }
    public int CurrentHealth { get; set; }

    /// <summary>
    /// Sets player stats back to their max values (i.e, health is back to its default value)
    /// </summary>
    public void ApplyUpgradeVals()
    {
        foreach (Upgrade upgrade in upgrades)
        {
            switch (upgrade.Type)
            {
                case UpgradeType.Health:
                    {
                        maxHealth += upgrade.Value;
                        //Debug.Log($"Current health: {stats.CurrentHealth}");
                        break;
                    }
                case UpgradeType.Damage:
                    {
                        // DAMAGE UPGRADE LOGIC HERE
                        Damage += upgrade.Value;
                        break;
                    }
            }
        }

        CurrentHealth = maxHealth;
    }
}
