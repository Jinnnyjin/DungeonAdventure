using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public RoomEventChannel roomEventChannel;
    public Room EnteringRoom;
    public MonsterSpawner spawner;
    public DungeonRenderer dungeonRenderer;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            roomEventChannel.Raise(EnteringRoom);
            Debug.Log($"방 입장: {EnteringRoom.Id}");

            RoomRuntimeData runtimeData = dungeonRenderer.GetRoomRuntimeData(EnteringRoom.Id);

            if (!runtimeData.isSpawned)
            {
                List<Vector2Int> spawnPositions = spawner.GetSpawnPositions(runtimeData.tileGrid, runtimeData.monsterPrefabs.Count);

                // 그럼 for문으로?

                for (int i = 0; i < runtimeData.monsterPrefabs.Count; i++)
                {
                    Vector3 spawnPos = dungeonRenderer.GetWorldPos(EnteringRoom, spawnPositions[i]);
                    Debug.Log(spawnPos);
                    spawner.SpawnMonster(runtimeData.monsterPrefabs[i], spawnPos);
                }
                runtimeData.isSpawned = true;
            }
        }
    }

}
