using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMan : MonoBehaviour
{
    [SerializeField] private GameObject deathUI;
    public GameObject DeathUI { get { return deathUI; } }

    public static SceneMan instance;

    private void Awake()
    {
        // Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deathUI.SetActive(false);
    }

    /// <summary>
    /// Loads next level based off of current level
    /// </summary>
    /// <param name="currentLevel">Build index of the scene of the current level</param>
    public void NextLevel(int currentLevel)
    {
        SceneManager.LoadScene(currentLevel + 1);
    }

    private void Update()
    {

    }

    public void ReturnToPreRunMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
}
