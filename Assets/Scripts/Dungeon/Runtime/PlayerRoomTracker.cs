using UnityEngine;

public class PlayerRoomTracker : MonoBehaviour
{
    public TileType CurrentPlayerTile { get; private set; } = TileType.Normal;

    [SerializeField] private DungeonRenderer dungeonRenderer;
    [SerializeField] private RoomEventChannel roomEnteredChannel;
    [SerializeField] private Transform playerTransform;

    private RoomRuntimeData curRuntimeData;

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
        curRuntimeData = dungeonRenderer.GetRoomRuntimeData(room.Id);
    }

    private void FixedUpdate()
    {
        if (curRuntimeData == null) return;

        Vector2Int playerLocalPos = dungeonRenderer.GetLocalPos(curRuntimeData.room, playerTransform.position);
        bool playerInBounds = playerLocalPos.x >= 0 && playerLocalPos.y >= 0
        && playerLocalPos.x < curRuntimeData.tileGrid.Width && playerLocalPos.y < curRuntimeData.tileGrid.Height;

        if(playerInBounds)
        {
            CurrentPlayerTile = curRuntimeData.tileGrid.GetTile(playerLocalPos);
            curRuntimeData.distanceField = curRuntimeData.tileGrid.ComputeDistanceField(playerLocalPos);
        }
    }
}
