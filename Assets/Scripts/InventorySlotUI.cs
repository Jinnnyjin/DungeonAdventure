using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;

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