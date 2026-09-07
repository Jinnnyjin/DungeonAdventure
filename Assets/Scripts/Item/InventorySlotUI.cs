using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{ 
    public int SlotIndex { get; private set; }

    [SerializeField] private Image iconImage;


    public void SetIndex(int index)
    {
        SlotIndex = index;
    }


    public void SetItem(ItemData item)
    {
        if (item == null)
        {
            iconImage.sprite = null;
            iconImage.color = new Color(1, 1, 1, 0);
        }

        else
        {
            iconImage.sprite = item.Icon;
            iconImage.color = new Color(1,1,1,1);
        }
    }
}