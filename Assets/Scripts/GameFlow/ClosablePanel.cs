using UnityEngine;

public class ClosablePanel : MonoBehaviour
{
    private PlayerInputActions playerInput;

    private void Start()
    {
        playerInput = PlayerActionManager.Instance.Actions;
    }

    // 닫기 버튼 클릭 또는 ESC 입력으로 패널을 비활성화
    private void Update()
    {
        if (playerInput.UI.Cancel.WasPressedThisFrame())
        {
            ClosePanel();
        }
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
