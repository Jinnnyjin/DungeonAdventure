using UnityEngine;

// 패널이 열리고 닫힐 때(Active 상태 변화) 게임을 일시정지/재개.
// 타이틀 씬 인스턴스에서는 pausesGame을 꺼두기
public class SettingsPanelController : MonoBehaviour
{
    [SerializeField] private bool pausesGame = false;

    private void OnEnable()
    {
        if (!pausesGame) return;

        // 일시정지
        Time.timeScale = 0f;

        if (PlayerActionManager.Instance != null)
        {
            PlayerActionManager.Instance.Actions.Player.Disable();
        }
    }

    private void OnDisable()
    {
        if (!pausesGame) return;

        Time.timeScale = 1f;

        if (PlayerActionManager.Instance != null)
        {
            PlayerActionManager.Instance.Actions.Player.Enable();
        }
    }
}
