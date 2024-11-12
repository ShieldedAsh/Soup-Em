using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopUpgradeManager : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Upgrade healthUpgrade;
    [SerializeField] private Upgrade damageUpgrade;

    [SerializeField] private GameObject moneyDisplayText;
    [SerializeField] private GameObject noMoneyText;
    [SerializeField] private GameObject upgradeDisplayText;

    private int damageUpgradeCount;
    private int healthUpgradeCount;

    private void Start()
    {
        SetMoneyText();

        damageUpgradeCount = 0;
        healthUpgradeCount = 0;

        // Determine totals at start
        foreach(Upgrade upgrade in playerStats.upgrades)
        {
            IncrementUpgradeTotal(upgrade.Type);
        }

        RefreshUpgradeDisplay();
    }

    /// <summary>
    /// Applies an upgrade to the player if they have enough money
    /// </summary>
    /// <param name="upgradeToApply">The upgrade to apply to the player</param>
    public void ApplyUpgrade(Upgrade upgradeToApply)
    {
        if (playerStats.Money - upgradeToApply.Cost >= 0)
        {
            playerStats.upgrades.Add(upgradeToApply);
            playerStats.Money -= upgradeToApply.Cost;
            SetMoneyText();

            IncrementUpgradeTotal(upgradeToApply.Type);
            RefreshUpgradeDisplay();
        }
        else
        {
            if (!noMoneyText.activeInHierarchy)
            {
                StartCoroutine(DisplayNoMoneyText());
            }
        }
    }

    //when the start run button is clicked, load the first level
    public void StartRun()
    {
        Debug.Log("Start run!");
        playerStats.ApplyUpgradeVals();
        SceneManager.LoadScene(1);
    }

    // TODO: Move this into an object that is present in every scene
    private void OnApplicationQuit()
    {
        playerStats.upgrades.Clear();
        playerStats.Money = 0;
    }

    private IEnumerator DisplayNoMoneyText()
    {
        noMoneyText.SetActive(true);
        yield return new WaitForSeconds(1);
        noMoneyText.SetActive(false);
    }

    private void SetMoneyText()
    {
        moneyDisplayText.GetComponent<TextMeshProUGUI>().text = $"Money - {playerStats.Money}";
    }

    // Increments upgrade totals based off of a provided type
    private void IncrementUpgradeTotal(UpgradeType type)
    {
        switch (type)
        {
            case UpgradeType.Health:
                {
                    healthUpgradeCount++;
                    break;
                }
            case UpgradeType.Damage:
                {
                    damageUpgradeCount++;
                    break;
                }
        }
    }

    // Refreshes the upgrade display so it displays current information
    private void RefreshUpgradeDisplay()
    {
        TextMeshProUGUI textComponent = upgradeDisplayText.GetComponent<TextMeshProUGUI>();
        textComponent.text = $"Current Upgrades:\n\n{damageUpgradeCount} Damage Upgrades (+{damageUpgradeCount} damage per shot)\n\n{healthUpgradeCount} Health upgrades (+{healthUpgradeCount*2} total health)";
    }
}
