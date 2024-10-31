using UnityEngine;

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour
{
    public static PlayerControls controls;

    private void Awake()
    {
        // Singleton
        if (controls == null)
        {
            controls = new PlayerControls();
        }
        else
        {
            Destroy(this);
        }

        
    }
}
