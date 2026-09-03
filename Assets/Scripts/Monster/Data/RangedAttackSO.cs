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

        Vector2 dir = (target.position - attacker.position).normalized;
        projectile.MoveDir = dir;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.Euler(0, 0, angle - 45f);

        projectile.SourcePrefab = ProjectilePrefab;
        projectile.MoveSpeed = ProjectileSpeed;
        projectile.Damage = Damage;
        projectile.Attacker = attacker;

        projectile.gameObject.layer = attacker.CompareTag("Player") ? LayerMask.NameToLayer("PlayerProjectile") : LayerMask.NameToLayer("MonsterProjectile");
    }
}
