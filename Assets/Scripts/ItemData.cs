using System.Collections.Generic;
using UnityEngine;

public enum EquipmentSlotType { Weapon, Armor, Accessory }

// 스탯을 수정해줄 구조체
[System.Serializable]
public struct StatModifier
{
    public StatType Type;
    public float Value;
}

[CreateAssetMenu(menuName ="Data/ItemData")]
public class ItemData : ScriptableObject
{
    public string ItemName;
    public Sprite Icon;
    public EquipmentSlotType SlotType;
    public JobType RequiredJob;
    public List<StatModifier> StatModifiers;
}
