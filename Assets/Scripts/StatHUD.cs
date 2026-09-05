using TMPro;
using UnityEngine;

public class StatHUD : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private VoidEventChannel onPlayerStatChangedChannel;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI attackText;
    [SerializeField] private TextMeshProUGUI defenseText;
    [SerializeField] private TextMeshProUGUI speedText;

    // PlayerStat의 OnEnable과 순서 맞추기 위해
    // PlayerStat onEnable 이후에 실행되도록 하기 위해서 Start에 둠!
    private void Start()
    {
        onPlayerStatChangedChannel.OnEventRaised += RefreshStatText;
        // 텍스트가 빈 채로 시작할 수 있으니 직접 첫 갱신
        RefreshStatText();
    }

    private void OnDisable()
    {
        onPlayerStatChangedChannel.OnEventRaised -= RefreshStatText;
    }

    private void RefreshStatText()
    {
        hpText.text = $"HP : {playerStats.CurHp} / {playerStats.MaxHealth}";
        attackText.text = $"ATK : {playerStats.Attack}";
        defenseText.text = $"DEF : {playerStats.Defense}";
        speedText.text = $"SPD : {playerStats.MoveSpeed}";
    }

}
