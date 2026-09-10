using UnityEngine;

[System.Serializable]
public struct RoomRatio
{
    public int MinCount;
    public int MaxCount;
    public float MeleeRatio;
}

public class MonsterSpawnAssigner 
{
    // 원거리, 근거리 따로
    private readonly GameObject meleePrefab;
    private readonly GameObject rangedPrefab;
    private readonly GameObject bossPrefab;

    private readonly RoomRatio normalRoomRatio;
    private readonly RoomRatio bossRoomRatio;

    public MonsterSpawnAssigner(GameObject meleePrefab, GameObject rangedPrefab, GameObject bossPrefab, RoomRatio normalRoomRatio, RoomRatio bossRoomRatio )
    {
        this.meleePrefab = meleePrefab;
        this.rangedPrefab = rangedPrefab;
        this.bossPrefab = bossPrefab;

        this.normalRoomRatio = normalRoomRatio;
        this.bossRoomRatio = bossRoomRatio;
    }

    public void AssignMonsters(DungeonGraph graph, RoomRuntimeRegistry roomRegistry)
    {
        foreach(var room in graph.AllRooms)
        {
            RoomRuntimeData runtimeData = roomRegistry.Get(room.Id);

            switch (room.Type)
            {
                case RoomType.Start:
                case RoomType.Treasure:
                    runtimeData.isCleared = true;
                    break;

                case RoomType.Normal:
                    AssignRandomMonsters(runtimeData, normalRoomRatio);
                    break;
                case RoomType.Boss:
                    runtimeData.monsterPrefabs.Add(bossPrefab);
                    AssignRandomMonsters(runtimeData, bossRoomRatio);
                    break;

            }
        }
    }

    private void AssignRandomMonsters(RoomRuntimeData runtimeData, RoomRatio ratio)
    {
        int count = Random.Range(ratio.MinCount, ratio.MaxCount + 1);

        for (int i = 0; i < count; i++)
        {
            if(Random.value < ratio.MeleeRatio)
            {
                runtimeData.monsterPrefabs.Add(meleePrefab);
            }
            else
            {
                runtimeData.monsterPrefabs.Add(rangedPrefab);
            }
        }
    }

}
