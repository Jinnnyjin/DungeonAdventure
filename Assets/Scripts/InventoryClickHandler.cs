using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryClickHandler : MonoBehaviour, IPointerClickHandler
{
    public Inventory Inventory { get; private set; }

    public void SetInventory(Inventory inventory)
    {
        Inventory = inventory;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        InventorySlotUI slotUI = GetComponent<InventorySlotUI>();

        int index = slotUI.SlotIndex;

        Inventory.EquipItem(index);
    }
}
