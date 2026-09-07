using UnityEngine;

public class EquipmentSlotRefresher : MonoBehaviour
{
    [SerializeField] private Inventory inventory;

    [SerializeField] private InventorySlotUI weaponSlot;
    [SerializeField] private InventorySlotUI armorSlot;
    [SerializeField] private InventorySlotUI accessorySlot;

    [SerializeField] private VoidEventChannel onItemEquippedChannel;
    [SerializeField] private VoidEventChannel onItemUnequippedChannel;

    private void OnEnable()
    {
        onItemEquippedChannel.OnEventRaised += RefreshEquipSlot;
        onItemUnequippedChannel.OnEventRaised += RefreshEquipSlot;
    }

    private void OnDisable()
    {
        onItemEquippedChannel.OnEventRaised -= RefreshEquipSlot;
        onItemUnequippedChannel.OnEventRaised -= RefreshEquipSlot;
    }

    private void RefreshEquipSlot()
    {
        ItemData weapon = inventory.GetEquippedWeapon();
        ItemData armor = inventory.GetEquippedArmor();
        ItemData accessory = inventory.GetEquippedAccessory();

        weaponSlot.SetItem(weapon);
        armorSlot.SetItem(armor);
        accessorySlot.SetItem(accessory);
    }
}
