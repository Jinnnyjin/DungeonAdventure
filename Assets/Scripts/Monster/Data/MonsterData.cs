using UnityEngine;

[CreateAssetMenu(menuName ="Data/Monster")]
public class MonsterData : ScriptableObject
{

    public string MonsterName;
    public int Health;
    public float MoveSpeed;
    public float DetectionRange;
    public AttackBehaviorSO AttackBehavior;

}
