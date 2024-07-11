using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using Mirror;
public class PlayerMove : NetworkBehaviour
{
    public float moveSpeed = 8f;
    public float turnSpeed = 180f;
    private Rigidbody rb;
    private Vector2 moveInput;

    public Animator anim;
    public float Speed;
    [Range(0, 1f)]
    public float StartAnimTime = 0.3f;
    [Range(0, 1f)]
    public float StopAnimTime = 0.15f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        PlayerInput controls = new PlayerInput();
        controls.Enable();
        controls.Character.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Character.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    void OnDisable()
    {
        PlayerInput controls = new PlayerInput();
        controls.Disable();
        moveInput = Vector2.zero;
    }
    
    void FixedUpdate()
    {
        Speed = moveInput.sqrMagnitude;

        if (Speed > 0.1f)
        {
            anim.SetFloat("Blend", Speed, StartAnimTime, Time.deltaTime);
        }
        else if (Speed < 0.1f)
        {
            anim.SetFloat("Blend", Speed, StopAnimTime, Time.deltaTime);
        }
    

        float targetAngle = transform.eulerAngles.y + moveInput.x * turnSpeed * Time.fixedDeltaTime;
        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
        Vector3 moveDirection = transform.forward * moveInput.y;
        rb.velocity = moveDirection * moveSpeed;
    }
}
