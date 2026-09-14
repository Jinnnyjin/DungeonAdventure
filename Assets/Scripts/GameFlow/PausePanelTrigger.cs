using UnityEngine;

// 패널이 꺼져있을 때만 ESC로 열어줌. 닫는 건 ClosablePanel이 담당(패널이 켜져있을 때만 반응)하므로 서로 겹치지 않음
public class PausePanelTrigger : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;

    private PlayerInputActions playerInput;

    private void Start()
    {
        playerInput = PlayerActionManager.Instance.Actions;
    }

    private void Update()
    {
        if (settingsPanel.activeSelf) return;

        if (playerInput.UI.Cancel.WasPressedThisFrame())
        {
            settingsPanel.SetActive(true);
        }
    }
}
