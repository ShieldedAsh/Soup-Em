using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerupControl : MonoBehaviour
{

    public TMP_Text text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            Debug.Log("All enemies dead");
            text.enabled = true;
        }
    }
}
