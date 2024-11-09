using UnityEngine;
using UnityEngine.SceneManagement;

public class PowerupControl : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            Debug.Log("All enemies dead");
            SceneManager.LoadScene("Upgrade Scene");
        }
    }
}
