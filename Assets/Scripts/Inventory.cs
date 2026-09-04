using System;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int inventorySize = 15;
    [SerializeField] private VoidEventChannel onItemEquippedChannel;
    [SerializeField] private VoidEventChannel onItemUnequippedChannel;
    [SerializeField] private ItemEventChannel onItemAcquiredChannel;
    [SerializeField] private ItemEventChannel onItemDiscardChannel;
    

    private ItemData[] slots;

    private ItemData equippedWeapon;
    private ItemData equippedArmor;
    private ItemData equippedAccessory;


    public int InventorySize => inventorySize;
    public ItemData GetEquippedWeapon() => equippedWeapon;
    public ItemData GetEquippedArmor() => equippedArmor;
    public ItemData GetEquippedAccessory() => equippedAccessory;

    public ItemData GetSlot(int index) => slots[index];

    public ItemData[] GetAllSlots()
    {
        // 복사본을 반환
        return (ItemData[])slots.Clone();
    }



    private void Awake()
    {
        slots = new ItemData[inventorySize];
    }


    // 빈칸 조회
    public bool HasEmptySlot()
    {
        foreach (var slot in slots)
        {
            if (slot == null) return true;
        }

        return false;
    }


    // 아이템 습득
    public bool TryAddItem(ItemData item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if(slots[i] == null)
            {
                slots[i] = item;
                onItemAcquiredChannel.Raise(item);
                return true;
            }
        }

        return false;
    }

    // 아이템 제거(버리기가 아님, 인벤토리에서 보이지않게 제거, 아이템 착용/버리기에 사용됨)
    public void RemoveItemAt(int index)
    {
        if (slots[index] == null)
        {
            throw new InvalidOperationException($"{index}번 슬롯은 이미 비어있음");
        }

        slots[index] = null;
    }


    // 아이템 착용
    public void EquipItem(int index)
    {
        ItemData item = slots[index];
        if (item == null) return;

        switch(item.SlotType)
        {
            case EquipmentSlotType.Weapon:
                RemoveItemAt(index);
                if(equippedWeapon != null)
                {
                    TryAddItem(equippedWeapon);
                }
                equippedWeapon = item;
                break;

            case EquipmentSlotType.Armor:
                RemoveItemAt(index);
                if (equippedArmor != null)
                {
                    TryAddItem(equippedArmor);
                }
                equippedArmor = item;
                break;

            case EquipmentSlotType.Accessory:
                RemoveItemAt(index);
                if (equippedAccessory != null)
                {
                    TryAddItem(equippedAccessory);
                }
                equippedAccessory = item;
                break;
        }

        onItemEquippedChannel.Raise();
    }

    // 아이템 착용 해제
    public bool TryUnEquip(EquipmentSlotType slotType)
    {
        switch(slotType)
        {
            case EquipmentSlotType.Weapon:
                if (equippedWeapon == null)
                {
                    throw new InvalidOperationException($"무기 장비 창에 착용한 장비가 없습니다.");
                }
                if(!TryAddItem(equippedWeapon))
                {
                    return false;
                }
                
                equippedWeapon = null;
                onItemUnequippedChannel.Raise();
                return true;

            case EquipmentSlotType.Armor:
                if (equippedArmor == null)
                {
                    throw new InvalidOperationException($"방어구 장비 창에 착용한 장비가 없습니다.");
                }

                if (!TryAddItem(equippedArmor))
                {
                    return false;
                }

                equippedArmor = null;
                onItemUnequippedChannel.Raise();
                return true;

            case EquipmentSlotType.Accessory:
                if (equippedAccessory == null)
                {
                    throw new InvalidOperationException($"악세사리 장비 창에 착용한 장비가 없습니다.");
                }

                if (!TryAddItem(equippedAccessory))
                {
                    return false;
                }

                equippedAccessory = null;
                onItemUnequippedChannel.Raise();
                return true;

            default:
                throw new ArgumentException($"알 수 없는 슬롯 타입: {slotType}");
        }
    }

    // 아이템 버리기
    public void DiscardItemAt(int index)
    {
        ItemData item = slots[index];
        RemoveItemAt(index);
        onItemDiscardChannel.Raise(item);
    }

    // 임시 디버그용 인벤토리 확인 메서드
    public void LogInventoryState()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            string itemName = slots[i] == null ? "비어있음" : slots[i].ItemName;
            Debug.Log($"슬롯 {i}: {itemName}");
        }
    }

    // 스탯 재계산
    public float SumModifiers(StatType type)
    {
        // 합계
        float total = 0;

        ItemData[] equippedItems = { equippedWeapon, equippedArmor, equippedAccessory };

        foreach (ItemData item in equippedItems)
        {
            if (item == null) continue;

            foreach (StatModifier modifier in item.StatModifiers)
            {
                if (modifier.Type == type)
                {
                    total += modifier.Value;
                }
            }
        }
        return total;
    }


}
