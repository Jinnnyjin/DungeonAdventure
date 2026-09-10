using UnityEngine;

public class DropSpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private OnMonsterKilledChannel onMonsterKilledChannel;

    private void OnEnable()
    {
        onMonsterKilledChannel.OnEventRaised += OnMonsterKilled;
    }

    private void OnDisable()
    {
        onMonsterKilledChannel.OnEventRaised -= OnMonsterKilled;
    }

    private void OnMonsterKilled(MonsterDeathInfo info)
    {
        DropTableRoller roller = new DropTableRoller();
        ItemData item = roller.RollDrop(info.MonsterData.DropTable);

        DroppedItem droppedItem = ObjectPoolManager.Instance.Get<DroppedItem>(prefab);
        droppedItem.transform.position = info.Position;
        droppedItem.SourcePrefab = prefab;
        droppedItem.SetItem(item);
    }
}
