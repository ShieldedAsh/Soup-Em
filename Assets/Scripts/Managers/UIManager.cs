using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private TextMeshProUGUI healthUIText;

    public static UIManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }


    public void UpdateHealthUI()
    {
        healthUIText.text = $"Health: {Mathf.Clamp(playerStats.CurrentHealth, 0, playerStats.MaxHealth)}";
    }
}
