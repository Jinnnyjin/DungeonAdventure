using UnityEngine;

public class DungeonTestRunner : MonoBehaviour
{
    [SerializeField] private  int minRooms;
    [SerializeField] private int maxRooms;
    [SerializeField] private int maxAttempts;

    void Start()
    {
        DungeonGenerator generator = new DungeonGenerator(minRooms,maxRooms,maxAttempts);

        DungeonGraph graph = generator.Generate();

        foreach (var rooms in graph.AllRooms)
        {
            Debug.Log($"Room {rooms.Id} ({rooms.Type}) at {rooms.GridPos}, 인접: {string.Join(",", rooms.ConnectedRoomIds)}");
        }
    }
}
