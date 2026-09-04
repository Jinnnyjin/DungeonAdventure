using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerItemPickUp : MonoBehaviour
{
    [SerializeField] private float pickUpRange;

    private Inventory inventory;
    private Collider2D[] hitBuffer = new Collider2D[10];
    private PlayerInputActions playerInput;

    private void Awake()
    {
        playerInput = new PlayerInputActions();
        inventory = GetComponent<Inventory>();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void Update()
    {
        if (playerInput.Player.PickUp.WasPressedThisFrame())
        {
            TryPickUp();
        }

        // 임시 디버그용
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            inventory.LogInventoryState();
        }
    }

    private void TryPickUp()
    {
        Debug.Log("TryPickUp 호출됨");

        int mask = LayerMask.GetMask("Item");
        Debug.Log($"Item 레이어 마스크 값: {mask}");

        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(LayerMask.GetMask("Item"));
        contactFilter.useTriggers = true;
        Debug.Log($"useLayerMask: {contactFilter.useLayerMask}, layerMask: {contactFilter.layerMask.value}");

        int itemCount = Physics2D.OverlapCircle(transform.position, pickUpRange, contactFilter, hitBuffer);
        Debug.Log($"itemCount: {itemCount}");

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
