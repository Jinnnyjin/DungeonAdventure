using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomDoorCalculator
{
    public List<Vector2Int> ComputeDoorPositions(Room room, DungeonGraph graph, int tileWidth, int tileHeight)
    {
        List<Vector2Int> doorPositions = new List<Vector2Int>();

        foreach (int id in room.ConnectedRoomIds )
        {
            Room connectedRoom = graph.GetRoom(id);
            //연결된 방과 기준 방 위치 비교
            Vector2Int direction = connectedRoom.GridPos - room.GridPos;

            if (direction == Vector2Int.right)
            {
                doorPositions.AddRange(CreateDoor(true, tileWidth - 1 , tileHeight));
            }
            else if (direction == Vector2Int.left)
            {
                doorPositions.AddRange(CreateDoor(true, 0, tileHeight));
            }
            else if (direction == Vector2Int.up)
            {
                doorPositions.AddRange(CreateDoor(false, tileHeight - 1, tileWidth));
            }
            else if (direction == Vector2Int.down)
            {
                doorPositions.AddRange(CreateDoor(false, 0, tileWidth));
            }
            else
            {
                throw new InvalidOperationException($"방향 설정이 잘못됨: {direction}");
            }
        }

        return doorPositions;
    }

    private List<Vector2Int> CreateDoor(bool fixedIsX, int fixedValue, int variableLength)
    {
        List<Vector2Int> doors = new List<Vector2Int>();

        int v1 = variableLength / 2;
        int v2 = variableLength / 2 -1 ;

        if (fixedIsX)
        {
            doors.Add(new Vector2Int(fixedValue, v1));
            doors.Add(new Vector2Int(fixedValue, v2));
        }
        else
        {
            doors.Add(new Vector2Int(v1, fixedValue));
            doors.Add(new Vector2Int(v2, fixedValue));
        }

        return doors;
    }


}
