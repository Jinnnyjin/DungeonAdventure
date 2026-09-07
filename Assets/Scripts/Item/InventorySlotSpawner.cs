using UnityEngine;
using System.Collections.Generic;

public class InventorySlotSpawner : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotContainer;
    private List<InventorySlotUI> slots = new List<InventorySlotUI>();

    public IEnumerable<InventorySlotUI> AllSlots => slots;

    private void Start()
    {
        for (int i = 0; i < inventory.InventorySize; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab,slotContainer);
            InventorySlotUI slot = slotObj.GetComponent<InventorySlotUI>();
            
            slot.SetIndex(i);

            slots.Add(slot);

            InventoryClickHandler handler = slotObj.AddComponent<InventoryClickHandler>();
            handler.SetInventory(inventory);
        }
    }

}
