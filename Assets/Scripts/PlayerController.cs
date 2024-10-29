using System.Collections;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph;
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

    private PlayerControls controls;
    private bool isDashing;
    private bool isAttacking;

    private void Awake()
    {
        controls = InputManager.controls;
        controls.Combat.Enable();

        controls.Combat.Dash.performed += Dash;
        isDashing = false;

        controls.Combat.Attack.performed += Attack;
        isAttacking = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MoveAction.Enable();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isDashing)
        {
            Move(controls.Combat.Move.ReadValue<Vector2>());
        }
    }

    private void Move(Vector2 moveInput)
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    private void Attack(InputAction.CallbackContext ctx)
    {
        // Your code here
    }

    private void Dash(InputAction.CallbackContext ctx)
    {
        if (rb.linearVelocity != Vector2.zero && !isDashing)
        {
            IEnumerator dashCoroutine = PerformDash();
            StartCoroutine(dashCoroutine);
        }
    }


    // This smells but eh, whatever
    private IEnumerator PerformDash()
    {
        isDashing = true;
        rb.linearVelocity *= 2;
        yield return new WaitForSeconds(.5f);
        rb.linearVelocity /= 2;
        isDashing = false;
    }
}

