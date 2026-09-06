using TMPro;
using UnityEngine;

public class DungeonCycleManager : MonoBehaviour
{
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
}
