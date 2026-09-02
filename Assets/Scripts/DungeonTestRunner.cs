using System.Collections.Generic;
using UnityEngine.InputSystem;
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

    [Header("현재 상황")]
    [SerializeField] private GameObject player;
    private Room currentRoom;
    [SerializeField] private GameObject meleePrefab;
    [SerializeField] private GameObject rangedPrefab;

    [Header("이벤트")]
    public RoomEventChannel roomEnteredChannel;
    public RoomEventChannel roomClearedChannel;

    void Start()
    {
        DungeonGenerator generator = new DungeonGenerator(minRooms,maxRooms,maxAttempts,useFixedSeed,seed);

        DungeonGraph graph = generator.Generate();

        DungeonTypeAssigner typeAssigner = new DungeonTypeAssigner();
        typeAssigner.AssignBossRoom(graph);
        typeAssigner.AssignTreasureRoom(graph);

        dungeonRenderer.RenderDungeon(graph);

        MonsterSpawnAssigner spawnAssigner = new MonsterSpawnAssigner(meleePrefab, rangedPrefab);
        spawnAssigner.AssignMonsters(graph, dungeonRenderer);

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
        Vector3 startPoint = dungeonRenderer.GetPlayerSpawnWorldPos(startRoom);
        player.transform.position = startPoint;
        currentRoom = startRoom; 

        // 현재 기준, Start방 id는 0고정
        Dictionary<int, int> distances = graph.ComputeDistances(0);

        foreach (var kvp in distances)
        {
            Debug.Log($"Room {kvp.Key} — 거리: {kvp.Value}");
        }
    }

    private void OnEnable()
    {
        roomEnteredChannel.OnEventRaised += OnRoomEntered;
    }

    private void OnDisable()
    {
        roomEnteredChannel.OnEventRaised -= OnRoomEntered;
    }

    private void OnRoomEntered(Room room)
    {
        currentRoom = room;
        Debug.Log(currentRoom);
    }

    private void Update()
    {
        if(Keyboard.current.fKey.wasPressedThisFrame)
        {
            roomClearedChannel.Raise(currentRoom);
        }

        if (Keyboard.current.gKey.wasPressedThisFrame)
        {
            RoomTileGrid tileGrid = dungeonRenderer.GetRoomRuntimeData(currentRoom.Id).tileGrid;
            Vector2Int testPos = new Vector2Int(tileGrid.Width / 2, tileGrid.Height / 2);
            int[,] distances = tileGrid.ComputeDistanceField(testPos);

            for (int y = tileGrid.Height - 1; y >= 0; y--)
            {
                string row = "";
                for (int x = 0; x < tileGrid.Width; x++)
                {
                    int d = distances[x, y];
                    row += (d == int.MaxValue ? "#" : d.ToString()) + "\t";
                }
                Debug.Log(row);
            }
        }
    }
}
