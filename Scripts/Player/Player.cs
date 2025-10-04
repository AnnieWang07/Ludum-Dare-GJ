using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Player : MonoBehaviour
{
    [Header("Move Info")]
    public float moveSpeed = 8f;
    public float jumpForce;

    [Header("Collision Info")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;

    [Header("Jump Info")]
    public int maxJumpCount = 1;
    public int jumpCount;
    public bool hasDoubleJump = true;
    public int facingDir { get; private set; } = 1;
    private bool facingRight = true;


    #region Components
    public Rigidbody2D rb { get; private set; }
    #endregion
    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }

    #endregion
    private void Awake()
    {
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        stateMachine.Initialize(idleState);

    }

    private void Update()
    {
        stateMachine.currentState.Update();
        Debug.Log(rb.linearVelocity);
    }

    public void SetVelocity(float x, float y)
    {
        rb.linearVelocity = new Vector2(x, y);
    }

    public void SetVelocityX(float x)
    {
        rb.linearVelocity = new Vector2(x, rb.linearVelocity.y);
    }

    public void SetVelocityY(float y)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, y);
    }

    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }

    public void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

    }

    public void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight)
            Flip();
    }
   
}
