using System.Collections.Generic;
using System.Text;
using UnityEditor.Toolbars;
using UnityEngine;

public class DungeonTestRunner : MonoBehaviour
{
    [Header("던전 그리드")]
    [SerializeField] private  int minRooms;
    [SerializeField] private int maxRooms;
    [SerializeField] private int maxAttempts;
    [SerializeField] private bool useFixedSeed;
    [SerializeField] private int seed;
    [SerializeField] private DungeonRenderer dungeonRenderer;

    [SerializeField] private GameObject player;

    void Start()
    {
        DungeonGenerator generator = new DungeonGenerator(minRooms,maxRooms,maxAttempts,useFixedSeed,seed);

        DungeonGraph graph = generator.Generate();

        DungeonTypeAssigner typeAssigner = new DungeonTypeAssigner();
        typeAssigner.AssignBossRoom(graph);
        typeAssigner.AssignTreasureRoom(graph);

        dungeonRenderer.RenderDungeon(graph);

        Room startRoom = null;
        foreach (var rooms in graph.AllRooms)
        {
            if(rooms.Type == RoomType.Start)
            {
                startRoom = rooms;
                break;
            }
        }

        // 시작방 센터 좌표
        Vector3 startPoint = dungeonRenderer.GetRoomCenterWorldPos(startRoom);
        player.transform.position = startPoint;

        // 현재 기준, Start방 id는 0고정
        Dictionary<int, int> distances = graph.ComputeDistances(0);

        foreach (var kvp in distances)
        {
            Debug.Log($"Room {kvp.Key} — 거리: {kvp.Value}");
        }
    }
}
