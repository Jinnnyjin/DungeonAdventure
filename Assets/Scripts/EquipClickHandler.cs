using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipClickHandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private EquipmentSlotType slotType;
    [SerializeField] private Inventory inventory;

    public void OnPointerClick(PointerEventData eventData)
    {
        inventory.TryUnEquip(slotType);
    }
}
