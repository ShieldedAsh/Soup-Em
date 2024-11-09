using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopUpgradeManager : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Upgrade healthUpgrade;
    [SerializeField] private Upgrade damageUpgrade;

    /// <summary>
    /// Applies an upgrade to the player if they have enough money
    /// </summary>
    /// <param name="upgradeToApply">The upgrade to apply to the player</param>
    public void ApplyUpgrade(Upgrade upgradeToApply)
    {
        if (playerStats.Money >= upgradeToApply.Cost)
        {
            playerStats.upgrades.Add(upgradeToApply);
            playerStats.Money -= upgradeToApply.Cost;
        }
    }

    public void StartRun()
    {
        Debug.Log("Start run!");
        SceneManager.LoadScene(1);
    }

    // TODO: Move this into an object that is present in every scene
    private void OnApplicationQuit()
    {
        playerStats.upgrades.Clear();
        playerStats.Money = 0;
    }
}
