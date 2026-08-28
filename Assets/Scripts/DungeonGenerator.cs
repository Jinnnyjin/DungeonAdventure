using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator
{
    // 설정 값
    private readonly int minRooms;
    private readonly int maxRooms;
    private readonly int maxAttempts;

    public DungeonGenerator(int minRooms, int maxRooms, int maxAttempts, bool useFixedSeed, int seed)
    {
        this.minRooms = minRooms;
        this.maxRooms = maxRooms;
        this.maxAttempts = maxAttempts;
        if (useFixedSeed) Random.InitState(seed);
    }

    public DungeonGraph Generate()
    {
        int nextId = 0;
        DungeonGraph graph = new DungeonGraph();
        Dictionary<Vector2Int, Room> occupied = new Dictionary<Vector2Int, Room>();
        Vector2Int currentPos = Vector2Int.zero;

        int targetCount = Random.Range(minRooms, maxRooms + 1);
        int attempts = 0;

        // 시작(초기) 방
        Room startRoom = new Room
        {
            Id = nextId,
            Type = RoomType.Start,
            GridPos = currentPos,
            Size = Vector2Int.one,
            ConnectedRoomIds = new List<int>()
        };

        nextId++;
        graph.AddRoom(startRoom);
        occupied.Add(currentPos, startRoom);
        
        // 방 생성 루프
        while(occupied.Count < targetCount && attempts < maxAttempts)
        {
            // 다음 방 위치
            Vector2Int dir = GridDirections.Direction[Random.Range(0, GridDirections.Direction.Length)];
            Vector2Int nextPos = currentPos + dir;

            // 현재 방
            Room currentRoom = occupied[currentPos];

            // 다음 방의 키가 딕셔너리에 등록되어있다면
            if (occupied.ContainsKey(nextPos))
            {
                // 다음 방(이미 존재함)
                Room existingRoom = occupied[nextPos];

                if (!currentRoom.ConnectedRoomIds.Contains(existingRoom.Id))
                {
                    currentRoom.ConnectedRoomIds.Add(existingRoom.Id);
                    existingRoom.ConnectedRoomIds.Add(currentRoom.Id);
                }
            }
            // 등록되어 있지 않다면
            else
            {
                Room nextRoom = new Room
                {
                    Id = nextId,
                    Type = RoomType.Normal,
                    GridPos = nextPos,
                    Size = Vector2Int.one,
                    ConnectedRoomIds = new List<int>()
                };

                nextId++;
                graph.AddRoom(nextRoom);
                occupied.Add(nextPos, nextRoom);

                currentRoom.ConnectedRoomIds.Add(nextRoom.Id);
                nextRoom.ConnectedRoomIds.Add(currentRoom.Id);
            }

            currentPos = nextPos;
            attempts++;
        }
        Debug.Log($"목표: {targetCount}, 실제: {occupied.Count}, 사용한 시도: {attempts}");

        return graph;
    }

}
