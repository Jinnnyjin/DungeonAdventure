using UnityEngine;

[CreateAssetMenu(menuName ="Data/MeleeAttack")]
public class MeleeAttackSO : AttackBehaviorSO
{
    public override void Attack(Transform attacker, Transform target)
    {
        IDamageable damageable = target.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage((int)Damage);
        }
    }
}
