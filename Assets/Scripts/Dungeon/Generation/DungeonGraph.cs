using System;
using System.Collections.Generic;

/// <summary>Room 그래프 저장소</summary>
public class DungeonGraph 
{
    private Dictionary<int, Room> rooms = new Dictionary<int, Room>();
    // 읽기 전용 순회용
    public IEnumerable<Room> AllRooms => rooms.Values;


    public Room GetRoom(int Id)
    {
        bool found = rooms.TryGetValue(Id, out Room room);

        if (!found)
        {
            throw new ArgumentException($"{Id} 번호를 가진 방은 없음");
        }

        return rooms[Id];
    }

    public void AddRoom(Room room)
    {
        if (rooms.ContainsKey(room.Id))
        {
            throw new ArgumentException($"Room with Id {room.Id} already exists.");
        }

        rooms[room.Id] = room;
    }

    public Dictionary<int, int> ComputeDistances(int fromRoomId)
    {
        Queue<int> roomIds = new Queue<int>();
        // 방 ID, 거리
        Dictionary<int, int> distances = new Dictionary<int, int>();

        distances.Add(fromRoomId, 0);
        roomIds.Enqueue(fromRoomId);

        while (roomIds.Count > 0)
        {
            int curRoomId = roomIds.Dequeue();
            Room room = GetRoom(curRoomId);

            // 연결된 방 순회, distances에 이미 있는지 확인
            foreach (int id  in room.ConnectedRoomIds)
            {
                if (distances.ContainsKey(id)) continue;

                distances.Add(id, distances[curRoomId] + 1);
                roomIds.Enqueue(id);
            }
        }

        return distances;
    }
}
