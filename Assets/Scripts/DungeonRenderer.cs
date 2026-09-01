using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonRenderer : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;

    [SerializeField] private TileBase floorTile;
    [SerializeField] private TileBase roughTile;
    [SerializeField] private TileBase barrierTile;

    [SerializeField] private TileBase wallUp;
    [SerializeField] private TileBase wallDown;
    [SerializeField] private TileBase wallLeft;
    [SerializeField] private TileBase wallRight;
    [SerializeField] private TileBase wallUpLeft;
    [SerializeField] private TileBase wallUpRight;
    [SerializeField] private TileBase wallDownLeft;
    [SerializeField] private TileBase wallDownRight;

    [SerializeField] private int tileWidth;
    [SerializeField] private int tileHeight;
    [SerializeField] private int maxAttempts;
    [SerializeField] private float wallRatio;
    [SerializeField] private float roughRatio;

    public RoomEventChannel roomEventChannel;

    public void RenderDungeon(DungeonGraph graph)
    {
        RoomDoorCalculator doorCalculator = new RoomDoorCalculator();
        TileGridGenerator gridGenerator = new TileGridGenerator(tileWidth, tileHeight, maxAttempts, wallRatio, roughRatio);
        DungeonGridConverter converter = new DungeonGridConverter();
        WallDirectionCalculator directionCalculator = new WallDirectionCalculator();

        // 그래프 내 각 방 순회
        foreach (Room room in graph.AllRooms)
        {
            // 문 위치 계산
            List<Vector2Int> doorPositions = doorCalculator.ComputeDoorPositions(room, graph, tileWidth, tileHeight);

            // 타일 그리드 생성
            RoomTileGrid roomTile = gridGenerator.Generate(doorPositions);

            // 오프셋 계산
            Vector2Int offset = converter.GetRoomOffset(room, tileWidth, tileHeight);
            Debug.Log($"Room {room.Id} offset: {offset}");

            GameObject roomMap = new GameObject("Room_" + room.Id);
            roomMap.transform.position = GetRoomCenterWorldPos(room);

            BoxCollider2D roomCollider = roomMap.AddComponent<BoxCollider2D>();
            roomCollider.isTrigger = true;
            roomCollider.size = new Vector2(tileWidth, tileHeight);

            RoomTrigger roomTrigger = roomMap.AddComponent<RoomTrigger>();
            roomTrigger.EnteringRoom = room;
            roomTrigger.roomEventChannel = roomEventChannel;

            // 방 칸 순회
            for (int x = 0; x < tileWidth; x++)
            {
                for (int y = 0; y < tileHeight; y++)
                {
                    Vector2Int localPos = new Vector2Int(x, y);
                    bool isBorder = localPos.x == 0 || localPos.x == tileWidth - 1
                        || localPos.y == 0 || localPos.y == tileHeight - 1;
                    TileType curType = roomTile.GetTile(localPos);

                    TileBase tile = null;

                    if (curType == TileType.Normal)
                    {
                        tile = floorTile;
                    }
                    else if (curType == TileType.Rough)
                    {
                        tile = roughTile;
                    }
                    else if (curType == TileType.Wall)
                    {

                        if (isBorder)
                        {
                            WallDirection dir = directionCalculator.GetWallDirection(localPos, tileWidth, tileHeight);
                            switch (dir)
                            {
                                case WallDirection.Up: tile = wallUp; break;
                                case WallDirection.Down: tile = wallDown; break;
                                case WallDirection.Left: tile = wallLeft; break;
                                case WallDirection.Right: tile = wallRight; break;
                                case WallDirection.UpRight: tile = wallUpRight; break;
                                case WallDirection.UpLeft: tile = wallUpLeft; break;
                                case WallDirection.DownRight: tile = wallDownRight; break;
                                case WallDirection.DownLeft: tile = wallDownLeft; break;
                            }
                        }
                        else
                        {
                            tile = barrierTile;
                        }
                    }

                    //분기 종료
                    Vector2Int worldPos = offset + localPos;
                    tilemap.SetTile(new Vector3Int(worldPos.x, worldPos.y, 0), tile);
                }
            }
        }
    }

    // TODO : 장애물 / ROUGH 타일 위 스폰 방지 필요(RoomTileGrid 저장 구조 만들 때 같이 처리)
    // 방의 중심 월드 좌표 구하는 함수(offset => 방 내 중앙 칸 => 월드좌표)
    public Vector3 GetRoomCenterWorldPos(Room room)
    {
        DungeonGridConverter gridconverter = new DungeonGridConverter();
        Vector2Int offset = gridconverter.GetRoomOffset(room, tileWidth, tileHeight);

        int x = offset.x + (tileWidth / 2);
        int y = offset.y + (tileHeight / 2);

        Vector3Int centerPos = new Vector3Int(x, y, 0);

        return tilemap.GetCellCenterWorld(centerPos);
    }
}
