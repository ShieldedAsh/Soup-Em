using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public GameObject button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //make sure next level button is not active when the game starts
        button.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //sets the button to active if all enemies in the level are dead
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0 && GameObject.FindGameObjectsWithTag("StrongEnemy").Length == 0)
        {
            Debug.Log("All enemies dead");
            button.SetActive(true);
        }
    }
}
