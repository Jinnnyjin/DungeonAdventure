using UnityEngine;

[CreateAssetMenu(menuName = "Data/RangedAttack")]
public class RangedAttackSO : AttackBehaviorSO
{
    public GameObject ProjectilePrefab;
    public float ProjectileSpeed;

    public override void Attack(Transform attacker, Transform target)
    {
        Debug.Log($"Attack 호출됨, Time: {Time.time}");

        Projectile projectile = ObjectPoolManager.Instance.Get<Projectile>(ProjectilePrefab);
        projectile.transform.position = attacker.position;

        // 콜라이더 기준으로
        Vector3 aimPoint = target.position;
        Collider2D targetCollider = target.GetComponent<Collider2D>();
        if (targetCollider != null)
        {
            aimPoint = targetCollider.bounds.center;
        }

        Vector2 dir = (aimPoint - attacker.position).normalized;

        projectile.MoveDir = dir;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.Euler(0, 0, angle - 45f);

        projectile.SourcePrefab = ProjectilePrefab;
        projectile.MoveSpeed = ProjectileSpeed;
        projectile.Damage = GetFinalDamage(attacker);
        projectile.Attacker = attacker;

        projectile.gameObject.layer = attacker.CompareTag("Player") ? LayerMask.NameToLayer("PlayerProjectile") : LayerMask.NameToLayer("MonsterProjectile");
    }
}
