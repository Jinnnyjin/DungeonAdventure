using UnityEngine;

public abstract class AttackBehaviorSO : ScriptableObject
{
    public float AttackRange;
    public float Damage;
    public float Cooldown;
    public bool IsSingleTarget;


    public abstract void Attack(Transform attacker, Transform target);

    protected float GetFinalDamage(Transform attacker)
    {
        PlayerStats stats = attacker.GetComponent<PlayerStats>();
        if (stats != null)
        {
            return Damage + stats.Attack;
        }
        else return Damage;
    }
}
