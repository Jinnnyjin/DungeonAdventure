using UnityEngine;

public struct MonsterDeathInfo
{
    public MonsterData MonsterData;
    public Vector3 Position;
}

[CreateAssetMenu(menuName = "Events/MonsterDeathInfoChannel")]
public class MonsterDeathInfoEventChannel : EventChannel<MonsterDeathInfo>
{
    
}
