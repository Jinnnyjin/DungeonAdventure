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

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Inventory.DiscardItemAt(index);
            // TODO: 실패 시 시스템 메시지 로그 호출
            return;
        }

        bool success = Inventory.TryEquipItem(index);
        // TODO: 실패 시 시스템 메시지 로그 호출
    }
}
