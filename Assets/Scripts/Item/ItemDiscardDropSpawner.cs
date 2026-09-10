using UnityEngine;

public class ItemDiscardDropSpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private ItemEventChannel onItemDiscardChannel;
    [SerializeField] private Transform playerTransform;

    private void OnEnable()
    {
        onItemDiscardChannel.OnEventRaised += OnItemDiscarded;
    }

    private void OnDisable()
    {
        onItemDiscardChannel.OnEventRaised -= OnItemDiscarded;
    }

    private void OnItemDiscarded(ItemData item)
    {
        DroppedItem droppedItem = ObjectPoolManager.Instance.Get<DroppedItem>(prefab);
        droppedItem.transform.position = playerTransform.position;
        droppedItem.SourcePrefab = prefab;
        droppedItem.SetItem(item);
    }
}
