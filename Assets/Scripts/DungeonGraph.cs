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
}
