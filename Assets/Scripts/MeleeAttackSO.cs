using UnityEngine;

[CreateAssetMenu(menuName ="Data/MeleeAttack")]
public class MeleeAttackSO : AttackBehaviorSO
{
    public override void Attack(Transform attacker, Transform target)
    {
        Debug.Log("근접 공격");
    }
}
