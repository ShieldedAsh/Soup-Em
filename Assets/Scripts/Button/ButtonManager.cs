using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public GameObject button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0 && GameObject.FindGameObjectsWithTag("StrongEnemy").Length == 0)
        {
            Debug.Log("All enemies dead");
            button.SetActive(true);
        }
    }
}
