using UnityEngine;

public class PlayerRoomTracker : MonoBehaviour
{
    public TileType CurrentPlayerTile { get; private set; } = TileType.Normal;

    [SerializeField] private DungeonRenderer dungeonRenderer;
    [SerializeField] private RoomEventChannel roomEnteredChannel;
    [SerializeField] private Transform playerTransform;

    private RoomRuntimeRegistry roomRegistry;
    private DungeonCoordinateConverter coordinateConverter;
    private RoomRuntimeData curRuntimeData;

    // 아직 한 번도 계산 안 한 상태를 표시하는 값 (방 진입 시 최초 1회는 무조건 계산되도록)
    private static readonly Vector2Int Unset = new Vector2Int(int.MinValue, int.MinValue);
    private Vector2Int lastComputedLocalPos = Unset;

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
        lastComputedLocalPos = Unset;

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

            // 쫓아올 몬스터가 없거나 플레이어가 이전과 같은 타일에 머물러 있으면
            // 결과가 동일하므로 재계산 건너뜀
            bool hasChasers = curRuntimeData.spawnedMonsters.Count > 0;
            bool playerMovedTile = playerLocalPos != lastComputedLocalPos;

            if (hasChasers && playerMovedTile)
            {
                curRuntimeData.distanceField = curRuntimeData.tileGrid.ComputeDistanceField(playerLocalPos);
                lastComputedLocalPos = playerLocalPos;
            }
        }
    }
}
