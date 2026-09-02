using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public AttackBehaviorSO attackBehavior;
    public PlayerMovement playerMovement;
    public Transform attackRangeTransform;
    [SerializeField] private float attackRangeDistance = 0.7f;
    [SerializeField] private float bodyHeightOffset = 0.5f;

    private Collider2D[] hitBuffer = new Collider2D[10];
    private PlayerInputActions playerInput;

    private void Awake()
    {
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

    private void Update()
    {
        attackRangeTransform.localPosition = playerMovement.LastMoveDir * attackRangeDistance + new Vector2(0, bodyHeightOffset);

        if(playerInput.Player.Attack.WasPressedThisFrame())
        {
            TryAttack();
        }
    }

    private void TryAttack()
    {
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(LayerMask.GetMask("Monster"));

        int hitCount = Physics2D.OverlapCircle(attackRangeTransform.position, attackBehavior.AttackRange, contactFilter, hitBuffer);

        for (int i = 0; i < hitCount; i++)
        {
                attackBehavior.Attack(transform, hitBuffer[i].transform);
        }
    }

    private void OnDrawGizmos()
    {
        if (attackRangeTransform == null || attackBehavior == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackRangeTransform.position, attackBehavior.AttackRange);
    }
}
