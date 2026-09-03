
using UnityEngine;

public class DungeonGridConverter
{
    public Vector2Int GetRoomOffset(Room room, int tileWidth, int tileHeight)
    {
        Vector2Int roomOffset = new Vector2Int();

        roomOffset.x = room.GridPos.x * tileWidth;
        roomOffset.y = room.GridPos.y * tileHeight;

        return roomOffset;

    }
}
