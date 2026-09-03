using UnityEngine;

public abstract class AttackBehaviorSO : ScriptableObject
{
    public float AttackRange;
    public float Damage;
    public float Cooldown;
    public abstract void Attack(Transform attacker, Transform target);
}
