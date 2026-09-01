using UnityEngine;

[CreateAssetMenu(menuName = "Data/RangedAttack")]
public class RangedAttackSO : AttackBehaviorSO
{
    public GameObject ProjectilePrefab;
    public float ProjectileSpeed;

    public override void Attack(Transform attacker, Transform target)
    {
        Debug.Log("원거리 공격");
    }
}
