using System.Collections.Generic;
using UnityEngine;

public enum RoomType { Normal, Start, Treasure, Boss }

public class Room 
{
    public int Id;                      // 고유 번호
    public RoomType type;               // 방 타입
    public Vector2Int gridPos;          // 
    public Vector2Int size;             // 방 크기
    public List<int> connectedRoomIds;  // 연결된 방
}


