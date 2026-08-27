using System.Collections.Generic;
using UnityEngine;

public enum RoomType { Normal, Start, Treasure, Boss }

public class Room 
{
    public int Id;                      // 고유 번호
    public RoomType Type;               // 방 타입
    public Vector2Int GridPos;          // 
    public Vector2Int Size;             // 방 크기
    public List<int> ConnectedRoomIds;  // 연결된 방
}


