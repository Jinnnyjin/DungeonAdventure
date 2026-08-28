using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D playerRb;
    private PlayerInputActions playerInput;

    // TODO : 추후 플레이어 스탯과 연동 예정
    [SerializeField] private float moveSpeed;

    void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerInput = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    void FixedUpdate()
    {
        Vector2 moveInput = playerInput.Player.Move.ReadValue<Vector2>();

        Vector2 velocity = moveInput * moveSpeed ;

        playerRb.linearVelocity = velocity;
    }
}
