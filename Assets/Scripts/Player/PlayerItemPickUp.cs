using UnityEngine;

public class PlayerItemPickUp : MonoBehaviour
{
    [SerializeField] private float pickUpRange;

    private Inventory inventory;
    private Collider2D[] hitBuffer = new Collider2D[10];
    private PlayerInputActions playerInput;


    private void Start()
    {
        inventory = GetComponent<Inventory>();
        playerInput = PlayerActionManager.Instance.Actions;
    }

    private void Update()
    {
        if (playerInput.Player.PickUp.WasPressedThisFrame())
        {
            TryPickUp();
        }
    }

    private void TryPickUp()
    {
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(LayerMask.GetMask("Item"));
        contactFilter.useTriggers = true;

        int itemCount = Physics2D.OverlapCircle(transform.position, pickUpRange, contactFilter, hitBuffer);

        if (itemCount == 0) return;

        float closest = float.MaxValue;
        Collider2D pickItem =  null;
        for (int i = 0; i < itemCount; i++)
        {
            Collider2D item = hitBuffer[i];
            float dist = Vector2.Distance(item.transform.position, transform.position);

            if (dist < closest)
            {
                closest = dist;
                pickItem = item;
            }
        }

        if (pickItem == null) return;
        if (!inventory.HasEmptySlot()) return;

        DroppedItem droppedItem = pickItem.GetComponent<DroppedItem>();
        ItemData itemData = droppedItem.PickUp();
        
        inventory.TryAddItem(itemData);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickUpRange);
    }
}
