using UnityEngine;

public class InventorySlotRefresher : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private InventorySlotSpawner slotSpawner;
    [SerializeField] private ItemEventChannel onItemAcquiredChannel;
    [SerializeField] private VoidEventChannel onItemEquippedChannel;
    [SerializeField] private VoidEventChannel onItemUnequippedChannel;
    [SerializeField] private ItemEventChannel onItemDiscardChannel;


    private void OnEnable()
    {
        onItemAcquiredChannel.OnEventRaised += RefreshInventory;
        onItemDiscardChannel.OnEventRaised += RefreshInventory;

        onItemEquippedChannel.OnEventRaised += RefreshInventory;
        onItemUnequippedChannel.OnEventRaised += RefreshInventory;
    }

    private void OnDisable()
    {
        onItemAcquiredChannel.OnEventRaised -= RefreshInventory;
        onItemDiscardChannel.OnEventRaised -= RefreshInventory;

        onItemEquippedChannel.OnEventRaised -= RefreshInventory;
        onItemUnequippedChannel.OnEventRaised -= RefreshInventory;
    }

    private void RefreshInventory()
    {
        ItemData[] inventorySlots = inventory.GetAllSlots();

        int index = 0;
        foreach (var slot in slotSpawner.AllSlots)
        {
            slot.SetItem(inventorySlots[index]);

            index++;
        }
        
    }

    private void RefreshInventory(ItemData item)
    {
        RefreshInventory();
    }
}