using UnityEngine;

public class PlayerRoomTracker : MonoBehaviour
{
    public TileType CurrentPlayerTile { get; private set; } = TileType.Normal;

    // 씬에 배치된 DungeonRenderer를 통해서만 RoomRegistry/CoordinateConverter에 접근 가능
    // (플레인 C# 서비스 클래스는 Inspector에 직접 드래그로 연결할 수 없음)
    [SerializeField] private DungeonRenderer dungeonRenderer;
    [SerializeField] private RoomEventChannel roomEnteredChannel;
    [SerializeField] private Transform playerTransform;

    private RoomRuntimeRegistry roomRegistry;
    private DungeonCoordinateConverter coordinateConverter;
    private RoomRuntimeData curRuntimeData;

    private void OnEnable()
    {
        roomRegistry = dungeonRenderer.RoomRegistry;
        coordinateConverter = dungeonRenderer.CoordinateConverter;
        roomEnteredChannel.OnEventRaised += OnRoomEntered;
    }

    private void OnDisable()
    {
        roomEnteredChannel.OnEventRaised -= OnRoomEntered;
    }

    private void OnRoomEntered(Room room)
    {
        curRuntimeData = roomRegistry.Get(room.Id);

        if(!curRuntimeData.isVisited)
        {
            curRuntimeData.isVisited = true;
            if(curRuntimeData.darknessOverlay != null)
            {
                curRuntimeData.darknessOverlay.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        if (curRuntimeData == null) return;

        Vector2Int playerLocalPos = coordinateConverter.GetLocalPos(curRuntimeData.room, playerTransform.position);
        bool playerInBounds = playerLocalPos.x >= 0 && playerLocalPos.y >= 0
        && playerLocalPos.x < curRuntimeData.tileGrid.Width && playerLocalPos.y < curRuntimeData.tileGrid.Height;

        if(playerInBounds)
        {
            CurrentPlayerTile = curRuntimeData.tileGrid.GetTile(playerLocalPos);
            curRuntimeData.distanceField = curRuntimeData.tileGrid.ComputeDistanceField(playerLocalPos);
        }
    }
}
