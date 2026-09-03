using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonTypeAssigner
{
    private const int ROOMS_PER = 3;


    public void AssignBossRoom(DungeonGraph graph)
    {
        int farDistance = -1;
        int roomId = -1;

        // 그래프 내 방에서
        foreach (var room in graph.AllRooms)
        {
            // 스타트 방을 찾아서
            if (room.Type == RoomType.Start)
            {
                Dictionary<int, int> distances = graph.ComputeDistances(room.Id);

                // 거리가 가장 먼 방을 찾아
                foreach (var distance in distances)
                {
                    if (farDistance < distance.Value)
                    {
                        roomId = distance.Key;
                        farDistance = distance.Value;
                    }
                }
                break;
            }
        }

        // 보스 방으로 설정
        Room bossRoom = graph.GetRoom(roomId);
        bossRoom.Type = RoomType.Boss;

    }

    public void AssignTreasureRoom(DungeonGraph graph)
    {
        List<Room> normalRooms = new List<Room>();

        foreach (var room in graph.AllRooms)
        {
            if(room.Type == RoomType.Normal)
            {
                normalRooms.Add(room);
            }
        }

        for(int i = normalRooms.Count - 1 ; i > 0; i-- )
        {
            int randomIndex = Random.Range(0, i + 1);

            Room tmp = normalRooms[randomIndex];
            normalRooms[randomIndex] = normalRooms[i];
            normalRooms[i] = tmp;

        }

        // 보물방의 수 방 개수 / 3 
        int treasureCount = Mathf.CeilToInt((float) normalRooms.Count / ROOMS_PER);

        for (int i = 0; i < treasureCount; i++)
        {
            normalRooms[i].Type = RoomType.Treasure;
        }

    }
}
