using UnityEngine;
using UnityEngine.Events;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private GameObject healthUpgradeButton;
    [SerializeField] private GameObject damageUpgradeButton;


    [SerializeField] private Upgrade healthUpgrade;
    [SerializeField] private Upgrade damageUpgrade;

    /// <summary>
    /// Adds a health upgrade to the player
    /// </summary>
    public void ApplyHealthUpgrade()
    {
        playerStats.upgrades.Add(healthUpgrade);
        healthUpgrade.Level++;
        Debug.Log("Health upgrade!");
    }

    /// <summary>
    /// Adds a damage upgrade to the player
    /// </summary>
    public void AppleDamageUpgrade()
    {
        playerStats.upgrades.Add(damageUpgrade);
        damageUpgrade.Level++;
        Debug.Log("Damage upgrade!");
    }

    private void OnApplicationQuit()
    {
        playerStats.upgrades.Clear();
    }
}
