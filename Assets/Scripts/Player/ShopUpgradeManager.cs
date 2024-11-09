using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopUpgradeManager : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Upgrade healthUpgrade;
    [SerializeField] private Upgrade damageUpgrade;

    public int sceneCounter = 0;

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
        // Code here
        switch (sceneCounter)
        {
            case 0:
                SceneManager.LoadScene("Level1");
                break;
            case 1:
                SceneManager.LoadScene("Level2");
                break;
            case 2:
                SceneManager.LoadScene("Level3");
                break;
            case 3:
                SceneManager.LoadScene("Level4");
                break;
            case 4:
                SceneManager.LoadScene("Level5");
                break;
        }

        sceneCounter++;
    }

    // TODO: Move this into an object that is present in every scene
    private void OnApplicationQuit()
    {
        playerStats.upgrades.Clear();
        playerStats.Money = 0;
    }
}
