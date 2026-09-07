using System;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public GameObject SourcePrefab;

    private ItemData item;
    private bool isReturned;
    private SpriteRenderer spriteRenderer;

    public static readonly List<DroppedItem> Active = new List<DroppedItem>();

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        isReturned = false;
        Active.Add(this);
    }

    private void OnDisable()
    {
        Active.Remove(this);
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
