using System;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public GameObject SourcePrefab;

    private ItemData item;
    private bool isReturned;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        isReturned = false;
    }

    public ItemData PickUp()
    {
        if (isReturned) throw new InvalidOperationException("이미 return된 아이템");
        isReturned = true;

        ItemData pickedUp = this.item;

        ObjectPoolManager.Instance.Release<DroppedItem>(SourcePrefab, this);

        return pickedUp;
    }

    public void SetItem(ItemData _item)
    {
        item = _item;
        spriteRenderer.sprite = _item.Icon;
    }

}
