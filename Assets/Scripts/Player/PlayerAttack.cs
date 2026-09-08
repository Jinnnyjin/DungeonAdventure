using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public AttackBehaviorSO attackBehavior;
    public PlayerMovement playerMovement;
    public Transform attackRangeTransform;
    [SerializeField] private float attackRangeDistance = 0.7f;
    [SerializeField] private float bodyHeightOffset = 0.5f;

    [SerializeField] private Inventory inventory;
    [SerializeField] private VoidEventChannel onItemEquippedChannel;
    [SerializeField] private VoidEventChannel onItemUnequippedChannel;



    private Collider2D[] hitBuffer = new Collider2D[10];
    private PlayerInputActions playerInput;

    private void Start()
    {
        playerInput = PlayerActionManager.Instance.Actions;
    }

    private void OnEnable()
    {
        onItemEquippedChannel.OnEventRaised += RefreshAttackBehavior;
        onItemUnequippedChannel.OnEventRaised += RefreshAttackBehavior;
    }

    private void OnDisable()
    {
        onItemEquippedChannel.OnEventRaised -= RefreshAttackBehavior;
        onItemUnequippedChannel.OnEventRaised -= RefreshAttackBehavior;
    }

    private void Update()
    {
        attackRangeTransform.localPosition = playerMovement.LastMoveDir * attackRangeDistance + new Vector2(0, bodyHeightOffset);

        if(playerInput.Player.Attack.WasPressedThisFrame())
        {
            TryAttack();
        }
    }

    private void RefreshAttackBehavior()
    {
        ItemData weapon = inventory.GetEquippedWeapon();
        attackBehavior = weapon != null ? weapon.WeaponBehavior : null;
    }

    private void TryAttack()
    {
        // 무기 해제 상태 시 공격하지 않음
        if (attackBehavior == null) return; 

        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(LayerMask.GetMask("Monster"));
        
        int hitCount = Physics2D.OverlapCircle(attackRangeTransform.position, attackBehavior.AttackRange, contactFilter, hitBuffer);

        // 싱글타겟 공격
        if(attackBehavior.IsSingleTarget)
        {
            float closestDist = float.MaxValue;
            Collider2D closestTarget = null;

            for(int i = 0; i < hitCount; i++)
            {
                float dist = (hitBuffer[i].transform.position - attackRangeTransform.position).sqrMagnitude;

                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestTarget = hitBuffer[i];
                }

            }
            if (closestTarget == null) return;

            attackBehavior.Attack(transform, closestTarget.transform);
        }
        // 광역 공격
        else
        {
            for (int i = 0; i < hitCount; i++)
            {
                    attackBehavior.Attack(transform, hitBuffer[i].transform);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (attackRangeTransform == null || attackBehavior == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackRangeTransform.position, attackBehavior.AttackRange);
    }
}
