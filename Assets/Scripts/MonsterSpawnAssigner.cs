using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnAssigner 
{
    // 원거리, 근거리 따로
    private readonly GameObject meleePrefab;
    private readonly GameObject rangedPrefab;

    public MonsterSpawnAssigner(GameObject meleePrefab, GameObject rangedPrefab)
    {
        this.meleePrefab = meleePrefab;
        this.rangedPrefab = rangedPrefab;
    }

    public void AssignMonsters(DungeonGraph graph, DungeonRenderer renderer)
    {
        foreach(var room in graph.AllRooms)
        {
            if (room.Type != RoomType.Normal)
            {
                renderer.GetRoomRuntimeData(room.Id).isCleared = true;
                continue;
            }

            // 우선은 실행되는지 확인 차 Normal방에만 근접 1마리씩
            RoomRuntimeData runtimeData = renderer.GetRoomRuntimeData(room.Id);
            runtimeData.monsterPrefabs.Add(meleePrefab);
            runtimeData.monsterPrefabs.Add(rangedPrefab);
        }
    }

}
