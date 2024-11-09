using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMan : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    /// <summary>
    /// Loads next level based off of current level
    /// </summary>
    /// <param name="currentLevel">Build index of the scene of the current level</param>
    public void NextLevel(int currentLevel)
    {
        SceneManager.LoadScene(currentLevel + 1);
    }
}
