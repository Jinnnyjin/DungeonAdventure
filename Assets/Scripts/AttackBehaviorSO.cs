using UnityEngine;

public abstract class AttackBehaviorSO : ScriptableObject
{
    public float AttackRange;
    public float Damage;
    public abstract void Attack(Transform attacker, Transform target);
}
