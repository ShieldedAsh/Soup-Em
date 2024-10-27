using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Movement actions
    public InputAction MoveAction;

    public Rigidbody2D rb;

    //reg movement vars
    public float moveSpeed;

    //dash var
    public float dashForce;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MoveAction.Enable();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 move = MoveAction.ReadValue<Vector2>();
        rb.linearVelocity = move * moveSpeed;

        //dashing
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            rb.AddForce(move * dashForce, ForceMode2D.Impulse);
        }

    }
}

