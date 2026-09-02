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

            Vector2Int playerLocalPos = dungeonRenderer.GetLocalPos(EnteringRoom, collision.transform.position);
            runtimeData.distanceField = runtimeData.tileGrid.ComputeDistanceField(playerLocalPos);

            if (!runtimeData.isSpawned)
            {
                List<Vector2Int> spawnPositions = spawner.GetSpawnPositions(runtimeData.tileGrid, runtimeData.monsterPrefabs.Count);

                for (int i = 0; i < runtimeData.monsterPrefabs.Count; i++)
                {
                    Vector3 spawnPos = dungeonRenderer.GetWorldPos(EnteringRoom, spawnPositions[i]);
                    Monster monster = spawner.SpawnMonster(runtimeData.monsterPrefabs[i], spawnPos);
                    monster.runtimeData = runtimeData;
                    monster.dungeonRenderer = dungeonRenderer;
                    monster.playerTransform = collision.transform;
                }
                runtimeData.isSpawned = true;
            }
        }
    }

}
