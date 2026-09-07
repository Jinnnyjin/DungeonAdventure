using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonCycleManager : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private DungeonRunManager dungeonRunManager;

    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TextMeshProUGUI resultText;

    [SerializeField] private VoidEventChannel onPlayerDeadChannel;
    [SerializeField] private RoomEventChannel onRoomClearChannel;


    private void Awake()
    {
        HideResult();
    }

    private void OnEnable()
    {
        onPlayerDeadChannel.OnEventRaised += LoseStage;
        onRoomClearChannel.OnEventRaised += WinStage;
    }

    private void OnDisable()
    {
        onPlayerDeadChannel.OnEventRaised -= LoseStage;
        onRoomClearChannel.OnEventRaised -= WinStage;
    }

    public void RestartGame()
    {
        // 결과창 비활성화
        HideResult();

        // 인벤토리 초기화
        inventory.ResetInventory();

        // 스탯 초기화
        playerStats.ResetStats();

        // 던전 초기화
        dungeonRunManager.RunDungeon();

        // 조작 켜기
        PlayerActionManager.Instance.Actions.Player.Enable();
    }

    private void LoseStage()
    {
        string message = "Lose..";
        DisplayResult(message);
    }

    private void WinStage(Room room)
    {
        if (room.Type == RoomType.Boss)
        {
            string message = "Win!!";
            DisplayResult(message);
        }
    }

    private void DisplayResult(string message)
    {
        PlayerActionManager.Instance.Actions.Player.Disable();
        resultPanel.SetActive(true);
        resultText.text = message;
    }

    private void HideResult()
    {
        resultPanel.SetActive(false);
    }

    public void LoadTitleScene()
    {
        SceneManager.LoadScene(SceneNames.Title);
    }
}
