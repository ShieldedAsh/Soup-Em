using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Scriptable Objects/Upgrade")]
public class Upgrade : ScriptableObject
{
    public enum UpgradeType
    {
        Health,
        Damage
    }

    [SerializeField] private UpgradeType type; // type of upgrade (code is hella smelly but eh, who cares for this proj)
    [SerializeField] private int value; // what value to apply in the upgrade
    [SerializeField] private int cost; // how much the upgrade costs

    public UpgradeType Type { get { return type; } }
    public int Value { get { return value; } }
    public int Cost { get { return cost; } }
}
