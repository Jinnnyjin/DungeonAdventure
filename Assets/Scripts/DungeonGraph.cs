
using System;
using System.Collections.Generic;

public class DungeonGraph 
{
    private Dictionary<int, Room> rooms = new Dictionary<int, Room>();
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
