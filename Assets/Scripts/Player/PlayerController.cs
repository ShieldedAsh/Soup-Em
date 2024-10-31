using System.Collections;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;

    //reg movement vars
    public float moveSpeed;
    [SerializeField] private int health;

    public int Health { get { return health; } set { health = value; } }

    //dash var
    public float dashForce;

    private PlayerControls controls;
    private bool isDashing;
    private bool isAttacking;
    
    public ProjectileBehavior ProjectilePrefab;
    public Transform LaunchOffset;

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
        if (!isAttacking)
        {
            IEnumerator attackCoroutine = ShootProjecctile();
            StartCoroutine(attackCoroutine);
        }
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

    private IEnumerator ShootProjecctile()
    {
        isAttacking = true;
        Vector2 launchPosition = transform.position;
        Vector2 moveInput = controls.Combat.Attack.ReadValue<Vector2>();

        if (moveInput != Vector2.zero)
        {
            launchPosition += moveInput;
        }
        else
        {
            launchPosition += Vector2.up;
        }

        ProjectileBehavior projectile = Instantiate(ProjectilePrefab, launchPosition, transform.rotation);
        projectile.SetTarget(moveInput);
        yield return new WaitForSeconds(.5f);
        isAttacking = false;
    }
}