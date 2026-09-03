using UnityEngine.Pool;
using UnityEngine;
using System.Collections.Generic;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] monsterPrefabs;
    private Dictionary<GameObject, ObjectPool<Monster>> monsterPool = new Dictionary<GameObject, ObjectPool<Monster>>();
    [SerializeField] private int minDistance;



    private void Awake()
    {
        foreach (GameObject monsterPrefab in monsterPrefabs)
        {
            ObjectPool<Monster> monster = new ObjectPool<Monster>
                (
                    createFunc: () => Instantiate(monsterPrefab).GetComponent<Monster>(),
                    actionOnGet: (monster) => monster.gameObject.SetActive(true),
                    actionOnRelease: (monster) => monster.gameObject.SetActive(false),
                    maxSize: 20
                );

            monsterPool[monsterPrefab] = monster;
        }
    }

    public Monster SpawnMonster(GameObject monsterPrefab, Vector3 worldPos)
    {
        Monster monster = monsterPool[monsterPrefab].Get();
        monster.transform.position = worldPos;
        return monster;
    }

    public void ReleaseMonster(GameObject monsterPrefab, Monster monster)
    {
        monsterPool[monsterPrefab].Release(monster);

    }

    public List<Vector2Int> GetSpawnPositions(RoomTileGrid tileGrid, int count)
    {
        MonsterSpawnPositionCalclulator calclulator = new MonsterSpawnPositionCalclulator();
        return calclulator.CalculateSpawnPositions(tileGrid, count, minDistance);
    }
}
