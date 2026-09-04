using UnityEngine;
using System.Collections.Generic;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private int minDistance;
    [SerializeField] private OnMonsterKilledChannel onMonsterKilledChannel;


    public Monster SpawnMonster(GameObject monsterPrefab, Vector3 worldPos)
    {
        Monster monster = ObjectPoolManager.Instance.Get<Monster>(monsterPrefab);
        monster.transform.position = worldPos;
        monster.onMonsterKilledChannel = onMonsterKilledChannel;
        return monster;
    }

    public void ReleaseMonster(GameObject monsterPrefab, Monster monster)
    {
        ObjectPoolManager.Instance.Release<Monster>(monsterPrefab, monster);
    }

    public List<Vector2Int> GetSpawnPositions(RoomTileGrid tileGrid, int count)
    {
        MonsterSpawnPositionCalculator calculator = new MonsterSpawnPositionCalculator();
        return calculator.CalculateSpawnPositions(tileGrid, count, minDistance);
    }
}
