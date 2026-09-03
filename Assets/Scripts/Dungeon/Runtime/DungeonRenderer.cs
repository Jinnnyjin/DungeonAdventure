using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonRenderer : MonoBehaviour
{
    [Header("타일")]
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

    [Header("그 외")]
    public RoomEventChannel roomEnterChannel;
    public RoomEventChannel roomClearChannel;
    private Dictionary<int, RoomRuntimeData> runData = new Dictionary<int, RoomRuntimeData>();
    [SerializeField] private MonsterSpawner monsterSpawner;

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

            // 방 콜라이더
            GameObject roomMap = new GameObject("Room_" + room.Id);
            roomMap.layer = LayerMask.NameToLayer("RoomBounds");
            roomMap.transform.position = GetRoomCenterWorldPos(room);

            BoxCollider2D roomcollider = roomMap.AddComponent<BoxCollider2D>();
            roomcollider.isTrigger = true;
            roomcollider.size = new Vector2(tileWidth - 3, tileHeight - 3);
            
            // 방 트리거
            RoomTrigger roomTrigger = roomMap.AddComponent<RoomTrigger>();
            roomTrigger.EnteringRoom = room;
            roomTrigger.roomEventChannel = roomEnterChannel;
            roomTrigger.dungeonRenderer = this;
            roomTrigger.spawner = monsterSpawner;

            // 방 RuntimeData 설정
            RoomRuntimeData roomRuntimeData = new RoomRuntimeData();
            roomRuntimeData.room = room;
            roomRuntimeData.doors = new List<GameObject>();
            roomRuntimeData.monsterPrefabs = new List<GameObject>();
            roomRuntimeData.tileGrid = roomTile;
            roomRuntimeData.spawnedMonsters = new List<Monster>();
            runData[room.Id] = roomRuntimeData;

            // RuntimeData -> doors
            foreach(Vector2Int doorLocalPos in doorPositions)
            {
                Vector2Int worldPos = offset + doorLocalPos;
                GameObject door = new GameObject();
                door.transform.position = tilemap.GetCellCenterWorld(new Vector3Int(worldPos.x, worldPos.y, 0));

                BoxCollider2D boxCollider = door.AddComponent<BoxCollider2D>();
                boxCollider.isTrigger = false;
                // 데이터 추가
                roomRuntimeData.doors.Add(door);

                // 문 이벤트 채널 연결
                DoorGate doorGate = door.AddComponent<DoorGate>();
                doorGate.room = room;
                doorGate.dungeonRenderer = this;
                doorGate.roomClearChannel = roomClearChannel;
                doorGate.roomEnterChannel = roomEnterChannel;
            }

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

        return tilemap.CellToWorld(centerPos);
    }

    // 노멀칸
    public Vector3 GetPlayerSpawnWorldPos(Room room)
    {
        DungeonGridConverter gridconverter = new DungeonGridConverter();
        Vector2Int offset = gridconverter.GetRoomOffset(room, tileWidth, tileHeight);

        Vector2Int localCenterPos = new Vector2Int(tileWidth / 2, tileHeight / 2);
        Vector2Int localSpawnPos = GetRoomRuntimeData(room.Id).tileGrid.FindNearestNormalTile(localCenterPos);

        Vector3Int worldCenterPos = new Vector3Int(localSpawnPos.x + offset.x, localSpawnPos.y +  offset.y, 0);
        Debug.Log($"중앙: {localCenterPos}, 보정된 스폰: {localSpawnPos}, 타일타입: {GetRoomRuntimeData(room.Id).tileGrid.GetTile(localSpawnPos)}");
        return tilemap.GetCellCenterWorld(worldCenterPos); 
    }

    public Vector3 GetWorldPos(Room room, Vector2Int localPos)
    {
        DungeonGridConverter converter = new DungeonGridConverter();
        Vector2Int offset = converter.GetRoomOffset(room, tileWidth, tileHeight);

        Vector3Int worldPos = new Vector3Int(localPos.x + offset.x, localPos.y + offset.y, 0);

        return tilemap.GetCellCenterWorld(worldPos);
    }
    
    // worldPos - offset = localPos
    public Vector2Int GetLocalPos(Room room, Vector3 worldPos)
    {
        Vector3Int pos = tilemap.WorldToCell(worldPos);

        DungeonGridConverter converter = new DungeonGridConverter();
        Vector2Int offset = converter.GetRoomOffset(room, tileWidth, tileHeight);

        Vector2Int localPos = new Vector2Int(pos.x - offset.x, pos.y - offset.y);

        return localPos;
    }


    public RoomRuntimeData GetRoomRuntimeData(int roomId)
    {
        return runData[roomId];
    }
}
