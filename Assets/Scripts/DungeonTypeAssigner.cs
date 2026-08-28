using System.Collections.Generic;
using Unity.Android.Gradle;

public class DungeonTypeAssigner
{
    
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
}
