using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    private PlayerInputActions playerInput;

    private void Start()
    {
        playerInput = PlayerActionManager.Instance.Actions;
    }

    private void Update()
    {
        if(playerInput.UI.ToggleInventory.WasPressedThisFrame())
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);

            if (inventoryPanel.activeSelf)
            {
                PlayerActionManager.Instance.Actions.Player.Disable();
            }
            else
            {
                PlayerActionManager.Instance.Actions.Player.Enable();
            }
        }

    }

}
