using UnityEngine;

public class DungeonTestRunner : MonoBehaviour
{
    [SerializeField] private  int minRooms;
    [SerializeField] private int maxRooms;
    [SerializeField] private int maxAttempts;
    [SerializeField] private bool useFixedSeed;
    [SerializeField] private int seed;

    [SerializeField] private DungeonRenderer dungeonRenderer;

    void Start()
    {
        DungeonGenerator generator = new DungeonGenerator(minRooms,maxRooms,maxAttempts,useFixedSeed,seed);

        DungeonGraph graph = generator.Generate();

        dungeonRenderer.RenderDungeon(graph);

        foreach (var rooms in graph.AllRooms)
        {
            Debug.Log($"Room {rooms.Id} ({rooms.Type}) at {rooms.GridPos}, 인접: {string.Join(",", rooms.ConnectedRoomIds)}");
        }
    }
}
