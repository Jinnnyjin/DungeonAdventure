using UnityEngine;

public struct MonsterDeathInfo
{
    public MonsterData MonsterData;
    public Vector3 Position;
}

[CreateAssetMenu(menuName = "Events/OnMonsterKilledChannel")]
public class OnMonsterKilledChannel : EventChannel<MonsterDeathInfo>
{
    
}
