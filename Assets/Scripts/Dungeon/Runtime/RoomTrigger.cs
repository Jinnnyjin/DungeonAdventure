using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public RoomEventChannel roomEventChannel;
    public Room EnteringRoom;
    public MonsterSpawner monsterSpawner;
    public TreasureSpawner treasureSpawner;
    public DungeonRenderer dungeonRenderer;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            roomEventChannel.Raise(EnteringRoom);
            Debug.Log($"방 입장: {EnteringRoom.Id}");

            RoomRuntimeData runtimeData = dungeonRenderer.GetRoomRuntimeData(EnteringRoom.Id);

            Vector2Int playerLocalPos = dungeonRenderer.GetLocalPos(EnteringRoom, collision.transform.position);
            runtimeData.distanceField = runtimeData.tileGrid.ComputeDistanceField(playerLocalPos);

            // 몬스터 스폰
            if (!runtimeData.isSpawned)
            {
                List<Vector2Int> spawnPositions = monsterSpawner.GetSpawnPositions(runtimeData.tileGrid, runtimeData.monsterPrefabs.Count);

                for (int i = 0; i < runtimeData.monsterPrefabs.Count; i++)
                {
                    Vector3 spawnPos = dungeonRenderer.GetWorldPos(EnteringRoom, spawnPositions[i]);
                    Monster monster = monsterSpawner.SpawnMonster(runtimeData.monsterPrefabs[i], spawnPos);
                    monster.runtimeData = runtimeData;
                    monster.dungeonRenderer = dungeonRenderer;
                    monster.playerTransform = collision.transform;
                    monster.spawner = monsterSpawner;
                    monster.sourcePrefab = runtimeData.monsterPrefabs[i];
                    runtimeData.spawnedMonsters.Add(monster);
                }
                runtimeData.isSpawned = true;
            }

            // 보물 스폰
            if(EnteringRoom.Type == RoomType.Treasure && !runtimeData.isLooted)
            {
                // 최소 
                int treasureCount = treasureSpawner.GetDropCount();
                List<Vector2Int> positions = treasureSpawner.GetSpawnPositions(runtimeData.tileGrid, treasureCount);

                for (int i = 0; i < treasureCount; i++)
                {
                    Vector3 spawnPos = dungeonRenderer.GetWorldPos(EnteringRoom, positions[i]);
                    treasureSpawner.SpawnTreasure(spawnPos);
                }
                runtimeData.isLooted = true;
            }
        }
    }

}
