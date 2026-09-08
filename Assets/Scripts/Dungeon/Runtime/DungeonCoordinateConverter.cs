using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>방 로컬 좌표 <-> 월드 좌표 변환 담당</summary>
public class DungeonCoordinateConverter
{
    private readonly Tilemap tilemap;
    private readonly int tileWidth;
    private readonly int tileHeight;

    public DungeonCoordinateConverter(Tilemap tilemap, int tileWidth, int tileHeight)
    {
        this.tilemap = tilemap;
        this.tileWidth = tileWidth;
        this.tileHeight = tileHeight;
    }

    // 방의 중심 월드 좌표 구하는 함수(offset => 방 내 중앙 칸 => 월드좌표)
    public Vector3 GetRoomCenterWorldPos(Room room)
    {
        Vector2Int offset = DungeonGeometry.GetRoomOffset(room, tileWidth, tileHeight);

        int x = offset.x + (tileWidth / 2);
        int y = offset.y + (tileHeight / 2);

        Vector3Int centerPos = new Vector3Int(x, y, 0);

        return tilemap.CellToWorld(centerPos);
    }

    public Vector3 GetWorldPos(Room room, Vector2Int localPos)
    {
        Vector2Int offset = DungeonGeometry.GetRoomOffset(room, tileWidth, tileHeight);

        Vector3Int worldPos = new Vector3Int(localPos.x + offset.x, localPos.y + offset.y, 0);

        return tilemap.GetCellCenterWorld(worldPos);
    }

    // worldPos - offset = localPos
    public Vector2Int GetLocalPos(Room room, Vector3 worldPos)
    {
        Vector3Int pos = tilemap.WorldToCell(worldPos);

        Vector2Int offset = DungeonGeometry.GetRoomOffset(room, tileWidth, tileHeight);

        return new Vector2Int(pos.x - offset.x, pos.y - offset.y);
    }
}
