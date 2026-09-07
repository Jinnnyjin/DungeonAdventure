using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.AdaptivePerformance.Provider.AdaptivePerformanceSubsystemDescriptor;

public class TreasureSpawner : MonoBehaviour
{
    [SerializeField] private GameObject dropItemPrefab;
    [SerializeField] private List<DropEntry> dropTable;
    [SerializeField] private int minCount;
    [SerializeField] private int maxCount;
    [SerializeField] private int minDistance;


    public List<Vector2Int> GetSpawnPositions(RoomTileGrid tileGrid, int count)
    {
        RoomPositionSelector calculator = new RoomPositionSelector();
        return calculator.CalculateSpawnPositions(tileGrid, count, minDistance);
    }

    public void SpawnTreasure(Vector3 worldPos)
    {
        DropTableRoller roller = new DropTableRoller();
        ItemData item = roller.RollDrop(dropTable);

        DroppedItem droppedItem = ObjectPoolManager.Instance.Get<DroppedItem>(dropItemPrefab);
        droppedItem.transform.position = worldPos;
        droppedItem.SourcePrefab = dropItemPrefab;
        droppedItem.SetItem(item);
    }

    public int GetDropCount()
    {
        return Random.Range(minCount, maxCount + 1);
    }
}
