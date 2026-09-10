using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerRoomTracker roomDistanceFieldManager;

    private static readonly int IsRunningHash = Animator.StringToHash("IsRunning");
    public Vector2 LastMoveDir {  get; private set; }

    private PlayerStats playerStats;
    private Rigidbody2D playerRb;
    private PlayerInputActions playerInput;
    private Animator playerAnimator;
    private SpriteRenderer playerSpriteRenderer;

    void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        playerStats = GetComponent<PlayerStats>();
    }

    private void Start()
    {
        playerInput = PlayerActionManager.Instance.Actions;
    }

    void FixedUpdate()
    {
        Vector2 moveInput = playerInput.Player.Move.ReadValue<Vector2>();

        if (moveInput.x > 0) playerSpriteRenderer.flipX = false;
        else if (moveInput.x < 0) playerSpriteRenderer.flipX = true;

        float speedMultiplier = roomDistanceFieldManager.CurrentPlayerTile == TileType.Rough
                                ? RoomTileGrid.ROUGH_SPEED_MULTIPLIER : 1f;

        Vector2 velocity = moveInput * playerStats.MoveSpeed * speedMultiplier;

        playerAnimator.SetBool(IsRunningHash, velocity != Vector2.zero);

        playerRb.linearVelocity = velocity;

        if (velocity != Vector2.zero)
        {
            LastMoveDir = moveInput.normalized;
        }
    }
}
