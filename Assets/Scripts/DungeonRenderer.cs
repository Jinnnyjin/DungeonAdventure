using System;
using UnityEngine;

public class DungeonRenderer : MonoBehaviour
{
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private float ratio;

    public void RenderDungeon(DungeonGraph graph)
    {
        foreach (var rooms in graph.AllRooms)
        {
            
            Vector3 worldPos = new Vector3(rooms.GridPos.x * ratio, rooms.GridPos.y * ratio, 0f);

            GameObject room = Instantiate(roomPrefab, worldPos, Quaternion.identity);
            SpriteRenderer roomColor = room.GetComponent<SpriteRenderer>();

            roomColor.color = GetRoomColor(rooms.Type);
        }
    }

    // 우선은 방 스프라이트 대신 색 변경
    private Color GetRoomColor(RoomType type)
    {
        switch (type)
        {
            case RoomType.Start:
                return Color.white;
            case RoomType.Treasure:
                return Color.yellow;
            case RoomType.Boss:
                return Color.black;
            default:
                return Color.blue;
        }
    }
}
