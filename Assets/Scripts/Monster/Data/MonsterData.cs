using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DropEntry
{
    public ItemData Item;
    public float DropWeight;
}

[CreateAssetMenu(menuName ="Data/Monster")]
public class MonsterData : ScriptableObject
{

    public string MonsterName;
    public int Health;
    public float MoveSpeed;
    public float DetectionRange;
    public AttackBehaviorSO AttackBehavior;

    public List<DropEntry> DropTable;

}
