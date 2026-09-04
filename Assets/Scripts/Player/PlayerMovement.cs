using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // TODO : 추후 플레이어 스탯과 연동 예정
    [SerializeField] private float moveSpeed;
    public Vector2 LastMoveDir {  get; private set; }

    private Rigidbody2D playerRb;
    private PlayerInputActions playerInput;

    void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        playerInput = PlayerActionManager.Instance.Actions;
    }

    void FixedUpdate()
    {
        Vector2 moveInput = playerInput.Player.Move.ReadValue<Vector2>();

        Vector2 velocity = moveInput * moveSpeed ;

        playerRb.linearVelocity = velocity;

        if (velocity != Vector2.zero)
        {
            LastMoveDir = moveInput.normalized;
        }
    }
}
